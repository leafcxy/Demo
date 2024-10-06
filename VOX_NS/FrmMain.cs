using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VOX_NS
{
    public partial class FrmMain : Form
    {
        private System.Timers.Timer timer;

        private SimpleTcpServer server;
        private SimpleTcpClient client;
        
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Logs.Write("hello");
            try
            {
                //根据脚本创建sqlite表
                bool flag = File.Exists("sqlite.db");
                if (flag == false)
                {
                    string sql = File.ReadAllText("ScanRecord.sql");
                    SqliteHelper.ExecuteNonQuery(sql);
                }
                
                //定时器，更新Smt_MiddleTable里的ip和时间
                timer = new System.Timers.Timer();
                timer.Enabled = true;
                timer.AutoReset = true;
                timer.Interval = 10 * 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Handler);
                MsMESHelper.ExecuteNonQuery($"update Smt_MiddleTable set voxclientip='{CommonHelper.GetClentip()}',voxupdatetime=getdate() where Line='{CommonHelper.linedata}'");
                
                //通过Smt_MiddleTable获取正在生产的工单和型号
                DataTable dt = MsMESHelper.ExecuteDataset(CommandType.Text, $"select * from Smt_MiddleTable where Line='{CommonHelper.linedata}'").Tables[0];
                if(dt.Rows.Count > 0)
                {
                    UpdateSingle(tbProofJobnum, dt.Rows[0]["OrderNumber"].ToString().Trim());
                    UpdateSingle(tbProofPartnum, dt.Rows[0]["ProductModel"].ToString().Trim());
                }
                else
                {
                    UpdateMultiple(tbScan, "Unable to find feeding information");
                }

                //TCP服务端,接收海康软件发送过来的数据
                server = new SimpleTcpServer($"{CommonHelper.TCPServerIPAdress}:{CommonHelper.TCPServerPort}");
                server.Events.ClientConnected += ClientConnected;
                server.Events.ClientDisconnected += ClientDisconnected;
                server.Events.DataReceived += DataReceived;
                server.Start();

                //TCP客户端,发送判断结果给海康软件
                client = new SimpleTcpClient($"{CommonHelper.TCPIPAdress}:{CommonHelper.TCPPort}");
                client.Settings.LocalEndpoint = new IPEndPoint(IPAddress.Parse(CommonHelper.TCPServerIPAdress), Convert.ToInt32(CommonHelper.TCPLocationPort));
                client.Keepalive.EnableTcpKeepAlives = true;
                client.Events.Connected += Connected;
                client.Events.Disconnected += Disconnected;
                client.Logger = Logger;
                client.Connect();

                // once connected to the server...
                //client.Send(Encoding.ASCII.GetBytes("Hello, world!"));
            }
            catch (Exception ex)
            {
                UpdateSingle(tbScan, string.Empty);
                UpdateMultiple(tbScan, string.Empty);
                UpdateMultiple(tbScan, ex.Message);
                Logs.Write(ex);
            }
        }

        private void Timer_Handler(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MsMESHelper.ExecuteNonQuery($"update Smt_MiddleTable set voxclientip='{CommonHelper.GetClentip()}',voxupdatetime=getdate() where Line='{CommonHelper.linedata}'");
            }
            catch(Exception ex)
            {
                Logs.Write(ex);
            }
        }

        private void UpdateSingle(Control Control, string updatestr)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        Control.Text = updatestr;
                    }));
                }
                else
                {
                    Control.Text = updatestr;
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }
        }

        private void UpdateMultiple(Control Control, string updatestr)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        Control.Text += updatestr + "\r\n";
                    }));
                }
                else
                {
                    Control.Text += updatestr + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }
        }

        #region TCP服务端事件
        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            Logs.Write($"[{e.IpPort}] client connected");
        }
        private void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            Logs.Write($"[{e.IpPort}] client disconnected: {e.Reason}");
        }
        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                Console.WriteLine($"[{e.IpPort}]: {Encoding.ASCII.GetString(e.Data.Array, 0, e.Data.Count)}");
                //Console.WriteLine(e.Dump());
                //返回结果
                string responeResult = Encoding.ASCII.GetString(e.Data.Array, 0, e.Data.Count);
                if (!string.IsNullOrEmpty(responeResult))
                {
                    //处理responeResult含有0-开始的情况
                    responeResult = responeResult.Replace("0-", "");
                    HandleResult(responeResult);
                }
                else
                {
                    UpdateSingle(tbScan, string.Empty);
                    //提示：未收到海康傳輸的條碼
                    UpdateMultiple(tbScan, string.Empty);
                    UpdateMultiple(tbScan, "Prompt: the barcode transmitted by Haikang has not been received");
                }
            }
            catch(Exception ex)
            {
                Logs.Write(ex);
            }
        }

        private void HandleResult(string responeResult)
        {
            lock (this)
            {
                try
                {
                    List<Dealdata> Dealdatalist = new List<Dealdata>();
                    string JobNum = "";

                    UpdateSingle(tbScan, string.Empty);
                    if (responeResult.Length > 0)
                    {
                        UpdateMultiple(tbScan, responeResult);

                        DbParameter[] parms = {
                                     RDBSStrategy.GenerateSqliteInParam("@responeResult",DbType.String, 65535, responeResult)
                                   };
                        SqliteHelper.ExecuteNonQuery(CommandType.Text, "insert into ScanRecord values(NULL,@responeResult,datetime('now','localtime'));", parms);

                        //通过Smt_MiddleTable获取正在生产的工单和型号
                        DataTable dtMiddleTable = MsMESHelper.ExecuteDataset(CommandType.Text, $"select * from Smt_MiddleTable where Line='{CommonHelper.linedata}'").Tables[0];
                        if (dtMiddleTable.Rows.Count > 0)
                        {
                            UpdateSingle(tbProofJobnum, dtMiddleTable.Rows[0]["OrderNumber"].ToString().Trim());
                            UpdateSingle(tbProofPartnum, dtMiddleTable.Rows[0]["ProductModel"].ToString().Trim());
                        }
                        else
                        {
                            UpdateMultiple(tbScan, "Unable to find feeding information");
                        }
                        //处理接收到的数据
                        string[] resultArray = responeResult.Substring(0, responeResult.Length - 1).Split(';');
                        foreach (string item in resultArray)
                        {
                            Dealdata ddata = new Dealdata();
                            ddata.PartNum = dtMiddleTable.Rows[0]["ProductModel"].ToString().Trim();
                            ddata.FZjobnum = dtMiddleTable.Rows[0]["OrderNumber"].ToString().Trim();
                            if (item.IndexOf(',') > -1)
                            {
                                ddata.TRIOLabelNo = item.Split(',')[0];
                                ddata.DateCode = item.Split(',')[1];
                                ddata.jobnum = JobNum = ddata.TRIOLabelNo.Length > 8 ? ddata.TRIOLabelNo.Trim().Substring(0, 8) : string.Empty;
                                ddata.createtime = DateTime.Now;
                                if (ddata.TRIOLabelNo.Length != 17)
                                {
                                    ddata.Remark = "UUT barcode format is incorrect";
                                    ddata.ScanResult = "Fail";
                                }
                                else
                                {
                                    if (CommonHelper.IsDatecode(ddata.DateCode) == false)
                                    {
                                        ddata.Remark = "Datecode format is incorrect";
                                        ddata.ScanResult = "Fail";
                                    }
                                    else
                                    {
                                        //插入数据
                                        string errmsg = "";
                                        bool flag = CommonHelper.InsertData(item, ddata.FZjobnum,ddata.PartNum,out errmsg);
                                        if (flag)
                                        {
                                            ddata.Remark = "Scan succeeded！";
                                            ddata.ScanResult = "Success";
                                        }
                                        else
                                        {
                                            ddata.Remark = errmsg;
                                            ddata.ScanResult = "Fail";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ddata.TRIOLabelNo = string.Empty;
                                ddata.DateCode = item;
                                ddata.Remark = "Scan failed！";
                                ddata.ScanResult = "Fail";
                                ddata.createtime = DateTime.Now;
                            }
                            Dealdatalist.Add(ddata);
                        }
                        UpdateSingle(this.valProdJobnum, JobNum);

                        //判断扫描结果,更新dgv
                        this.Invoke((EventHandler)(delegate
                        {
                            dgvPass.Rows.Clear();
                            dgvFailed.Rows.Clear();
                        }));
                        foreach (Dealdata ddata in Dealdatalist)
                        {
                            if(ddata.ScanResult == "Success")
                            {
                                this.Invoke((EventHandler)(delegate
                                {
                                    dgvPass.Rows.Add(ddata.TRIOLabelNo, ddata.DateCode, ddata.Remark, ddata.createtime.ToString("HH:mm:ss"));
                                }));
                            }
                            else
                            {
                                this.Invoke((EventHandler)(delegate
                                {
                                    dgvFailed.Rows.Add(ddata.TRIOLabelNo, ddata.DateCode, ddata.Remark);
                                }));
                            }
                        }

                        //汇总扫描结果
                        int countSuccess = Dealdatalist.Where(x => x.ScanResult == "Success").ToList().Count;
                        int countFail = Dealdatalist.Where(x => x.ScanResult == "Fail").ToList().Count;
                        if (countFail > 0)
                        {
                            UpdateSingle(valScan, $"FAILED,Pass:{countSuccess},Failed:{countFail}");
                            client.Send(Encoding.ASCII.GetBytes("FAILED"));
                        }
                        else
                        {
                            UpdateSingle(valScan, $"SUCCEED,Pass:{countSuccess},Failed:{countFail}");
                            client.Send(Encoding.ASCII.GetBytes("SUCCEED"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    client.Send(Encoding.ASCII.GetBytes("FAILED"));
                    UpdateSingle(tbScan, string.Empty);
                    UpdateMultiple(tbScan, string.Empty);
                    UpdateMultiple(tbScan, "Error: " + ex.Message);
                    Logs.Write(ex);
                }
            }
        }

        #endregion

        #region TCP客户端事件
        private void Connected(object sender, ConnectionEventArgs e)
        {
            Logs.Write($"*** Server {e.IpPort} connected");
        }
        private void Disconnected(object sender, ConnectionEventArgs e)
        {
            Logs.Write($"*** Server {e.IpPort} disconnected");
            client.Dispose();
            Thread.Sleep(10 * 1000);
            client = new SimpleTcpClient($"{CommonHelper.TCPIPAdress}:{CommonHelper.TCPPort}");
            client.Settings.LocalEndpoint = new IPEndPoint(IPAddress.Parse(CommonHelper.TCPServerIPAdress), Convert.ToInt32(CommonHelper.TCPLocationPort));
            client.Keepalive.EnableTcpKeepAlives = true;
            client.Events.Connected += Connected;
            client.Events.Disconnected += Disconnected;
            client.Logger = Logger;
            client.ConnectWithRetries();
        }
        private void Logger(string msg)
        {
            Logs.Write(msg);
        }
        #endregion

        private void btnSend_Click(object sender, EventArgs e)
        {
            string result = string.Empty;

            //把dgv的数据拼接整理
            foreach(DataGridViewRow dgvr in dgvFailed.Rows)
            {
                if(dgvr.Cells["colFailedUUT"].Value == null || dgvr.Cells["colFailedDatecode"].Value == null)
                {
                    continue;
                }
                string strLabelno = dgvr.Cells["colFailedUUT"].Value == null ? "" : dgvr.Cells["colFailedUUT"].Value.ToString().Trim().ToLower();
                string strDatecode = dgvr.Cells["colFailedDatecode"].Value == null ? "" : dgvr.Cells["colFailedDatecode"].Value.ToString().Trim().ToLower();
                result += $"{strLabelno},{strDatecode};";
            }
            Console.WriteLine(result);

            if(result == "")
            {
                client.Send(Encoding.ASCII.GetBytes("SUCCEED"));
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(CallbackFunc), result);
            }
        }

        private void CallbackFunc(object obj)
        {
            HandleResult(obj.ToString());
            //this.Invoke((EventHandler)(delegate
            //{
            //    dgvPass.Rows.Add("1", "2", "3", "4");
            //    dgvFailed.Rows.Add("1", "2", "3");
            //}));
        }

        private void dgvFailed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 检查是否点击了删除按钮
            if (e.ColumnIndex == dgvFailed.Columns["colFailedOp"].Index && e.RowIndex >= 0)
            {
                // 获取被点击的行
                DataGridViewRow row = dgvFailed.Rows[e.RowIndex];

                // 删除行
                dgvFailed.Rows.Remove(row);
            }
        }
    }
}
