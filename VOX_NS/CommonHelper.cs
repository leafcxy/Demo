using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;

namespace VOX_NS
{
    public class CommonHelper
    {
        //配置文件的值
        public static string linedata = ConfigurationManager.AppSettings["Line"].ToString();
        public static string workShop = ConfigurationManager.AppSettings["WorkShop"].ToString();

        //TCP服务端
        public static string TCPServerIPAdress = ConfigurationManager.AppSettings["TCPServerIPAdress"].ToString();
        public static string TCPServerPort = ConfigurationManager.AppSettings["TCPServerPort"].ToString();

        //TCP客户端
        public static string TCPIPAdress = ConfigurationManager.AppSettings["TCPIPAdress"].ToString();
        public static string TCPPort = ConfigurationManager.AppSettings["TCPPort"].ToString();
        public static string TCPLocationPort = ConfigurationManager.AppSettings["TCPLocationPort"].ToString();

        //上料表缓存

        /// <summary>
        /// 获得字符串的长度,一个汉字的长度为1
        /// </summary>
        public static int GetStringLength(string s)
        {
            if (!string.IsNullOrEmpty(s))
                return Encoding.Default.GetBytes(s).Length;

            return 0;
        }

        /// <summary>
        /// 获得字符串中指定字符的个数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="c">字符</param>
        /// <returns></returns>
        public static int GetCharCount(string s, char c)
        {
            if (s == null || s.Length == 0)
                return 0;
            int count = 0;
            foreach (char a in s)
            {
                if (a == c)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 获得指定顺序的字符在字符串中的位置索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="order">顺序</param>
        /// <returns></returns>
        public static int IndexOf(string s, int order)
        {
            return IndexOf(s, '-', order);
        }

        /// <summary>
        /// 获得指定顺序的字符在字符串中的位置索引
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="c">字符</param>
        /// <param name="order">顺序</param>
        /// <returns></returns>
        public static int IndexOf(string s, char c, int order)
        {
            int length = s.Length;
            for (int i = 0; i < length; i++)
            {
                if (c == s[i])
                {
                    if (order == 1)
                        return i;
                    order--;
                }
            }
            return -1;
        }

        #region 分割字符串

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">分隔字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr, string splitStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
                return new string[0] { };

            if (sourceStr.IndexOf(splitStr) == -1)
                return new string[] { sourceStr };

            if (splitStr.Length == 1)
                return sourceStr.Split(splitStr[0]);
            else
                return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string sourceStr)
        {
            return SplitString(sourceStr, ",");
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="startIndex">开始位置的索引</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.Length >= (startIndex + length))
                    return sourceStr.Substring(startIndex, length);
                else
                    return sourceStr.Substring(startIndex);
            }

            return "";
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        public static string SubString(string sourceStr, int length)
        {
            return SubString(sourceStr, 0, length);
        }

        #endregion

        #region 移除前导/后导字符串

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr)
        {
            return TrimStart(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimStart(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Remove(0, trimStr.Length);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr)
        {
            return TrimEnd(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimEnd(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr) || !sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                return sourceStr;

            return sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr)
        {
            return Trim(sourceStr, trimStr, true);
        }

        /// <summary>
        /// 移除前导和后导字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">移除字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string Trim(string sourceStr, string trimStr, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(sourceStr))
                return string.Empty;

            if (string.IsNullOrEmpty(trimStr))
                return sourceStr;

            if (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Remove(0, trimStr.Length);

            if (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                sourceStr = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);

            return sourceStr;
        }

        #endregion

        /// <summary>
        /// 获取本机ip
        /// </summary>
        /// <returns></returns>
        public static string GetClentip()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < hostEntry.AddressList.Length; i++)
                {
                    if (hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostEntry.AddressList[i].ToString();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Vox日志记录插入开始时间和结束时间
        /// </summary>
        /// <param name="type"></param>
        /// <param name="TRIOLabelNo"></param>
        /// <param name="remarks"></param>
        public static void SaveDateTimeLog(string type, string TRIOLabelNo, string remarks)
        {
            try
            {
                string clientip = GetClentip();
                string strGuid = Guid.NewGuid().ToString();
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("insert into Engg.SmtProof_VoxLabel_MtlLotNum_DateTimeLog");
                sbSql.Append("(GUID, TRIOLabelNo, Ip, FailType, Remarks, CreatTime, UpadateTime) ");
                sbSql.Append($"values('{strGuid}','{TRIOLabelNo}','{clientip}','{type}','{remarks}',getdate(),getdate())");
                MsTrioHelper.ExecuteNonQuery(sbSql.ToString());
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }
        }

        /// <summary>
        /// Vox日志记录插入
        /// </summary>
        /// <param name="type"></param>
        /// <param name="TRIOLabelNo"></param>
        /// <param name="PartNum"></param>
        /// <param name="remarks"></param>
        public static void SaveFailLog(string type, string TRIOLabelNo, string PartNum, string remarks)
        {
            try
            {
                string clientip = GetClentip();
                string strGuid = Guid.NewGuid().ToString();
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("insert into Engg.SmtProof_VoxLabel_MtlLotNum_FailLog");
                sbSql.Append("(GUID, TRIOLabelNo, PartNum, Ip, FailType, Remarks, Isdelete, CreatTime, UpadateTime) ");
                sbSql.Append($"values('{strGuid}','{TRIOLabelNo}','{PartNum}','{clientip}','{type}','{remarks}','0',getdate(),getdate())");
                MsTrioHelper.ExecuteNonQuery(sbSql.ToString());
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }
        }

        /// <summary>
        /// 判断上料表数据是否一致
        /// </summary>
        /// <param name="ProofList"></param>
        /// <param name="PartNum"></param>
        /// <param name="line"></param>
        /// <param name="TRIOLabelNo"></param>
        /// <param name="jobnum"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsSameMaterials(List<Material> ProofList, string PartNum, string line, string TRIOLabelNo, string jobnum, out string error)
        {
            bool isSame = true;
            error = string.Empty;
            try
            {
                //以下是找出对应型号，在对应生产线使用中的上料表
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select b.Position,b.PNo from SmtProof_Feeding a ");
                sbSql.Append("inner join SmtProof_FeedingDetails b ");
                sbSql.Append("on a.FeedingID = b.FeedingID ");
                sbSql.Append($"where a.Model = N'{PartNum}' and a.MachineNo = N'{line}' and a.[Status]=N'使用中' ");
                sbSql.Append($"and b.Position not in (select Postion from [MES].[Engg].SmtProof_TempSUB2 where Model='{PartNum}' ");
                sbSql.Append($"AND CancelPosition=1 and MachineNo='{line}' and JobNo ='{jobnum}') order by Num ASC");

                List<Material> PeTableList = MsMESHelper.ExecuteDataset(CommandType.Text, sbSql.ToString()).Tables[0]
                    .AsEnumerable()
                    .Select(row => new Material{
                        Position = row.Field<string>("Position"),
                        PNo = row.Field<string>("PNo")
                    }).ToList();

                //添加到缓存

                if (PeTableList.Count > 0)
                {
                    //排序
                    ProofList = ProofList.OrderBy(a => a.Position).ToList();
                    PeTableList = PeTableList.OrderBy(a => a.Position).ToList();
                    //比较
                    if (ProofList.Count == PeTableList.Count)
                    {
                        for (int i = 0; i < ProofList.Count; i++)
                        {
                            if (ProofList[i].Position != PeTableList[i].Position)
                            {
                                error = $"[Different material locations]ProofPosition:{ProofList[i].Position};PeTablePosition:{PeTableList[i].Position};";
                                isSame = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        error = $"[The quantity of materials is different]ProofList.Count:{ProofList.Count};PeTableList.Count:{PeTableList.Count};";
                        isSame = false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = $"[program error]{ex.Message.ToString()}";
                isSame = false;
                
            }
            if (!isSame)
            {
                Logs.Write($"[{TRIOLabelNo}]PartNum:{PartNum};line:{line};");//用于调试
            }
            return isSame;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="joblistOrderNumber"></param>
        /// <param name="PartNum"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool InsertData(string item, string OrderNumber, string PartNum, out string errmsg)
        {
            bool result = false;
            bool checkSame = true;
            errmsg = string.Empty;

            string voxcode = string.Empty;
            string dateCode = string.Empty;
            string jobnum = string.Empty;
            try
            {
                if (item.IndexOf(',') > -1)
                {
                    voxcode = item.Split(',')[0];
                    dateCode = item.Split(',')[1];
                    jobnum = voxcode.Substring(0, 8);
                }
                else
                {
                    voxcode = item;
                    dateCode = "";
                }

                if (voxcode.Length > 8)
                {
                    //判斷防錯的工單和掃碼的工單
                    if (OrderNumber != jobnum)
                    {
                        Console.WriteLine($"[Inconsistent jobnum]OrderNumber:{OrderNumber};jobnum:{jobnum};");
                    }
                    if (PartNum == "")
                    {
                        //无对应工单型号
                        checkSame = false;
                        SaveFailLog("Missing data", voxcode, PartNum, "No corresponding work order model");
                        errmsg = "No corresponding work order model";
                        return result = false;
                    }

                    string strSql = $"select MaterialID,Model,PNo,Position,[Date] as FeedingDate from SmtProof_FeedingMaterialList where JobNo='{jobnum}' and Model='{PartNum}' and [Status]='生產中' and Surplus>=Dosage";
                    List<ProofRecord> feedingdata = MsMESHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0]
                    .AsEnumerable()
                    .Select(row => new ProofRecord{
                        MaterialID = row.Field<string>("MaterialID"),
                        Model = row.Field<string>("Model"),
                        PNo = row.Field<string>("PNo"),
                        Position = row.Field<string>("Position"),
                        FeedingDate = row.Field<DateTime>("FeedingDate")
                    }).ToList();
                    if (feedingdata.Count == 0)
                    {
                        strSql = $"select MaterialID,Model,PNo,Position,[Date] as FeedingDate from SmtProof_FeedingMaterialList where JobNo='{OrderNumber}' and Model='{PartNum}' and [Status]='生產中' and Surplus>=Dosage";
                        feedingdata = MsMESHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0]
                        .AsEnumerable()
                        .Select(row => new ProofRecord{
                            MaterialID = row.Field<string>("MaterialID"),
                            Model = row.Field<string>("Model"),
                            PNo = row.Field<string>("PNo"),
                            Position = row.Field<string>("Position"),
                            FeedingDate = row.Field<DateTime>("FeedingDate")
                        }).ToList();
                    }

                    //string strSql = $"select MaterialID,Model,PNo,Position,[Date] as FeedingDate from SmtProof_FeedingMaterialList where JobNo='{OrderNumber}' and Model='{PartNum}' and [Status]='生產中' and Surplus>=Dosage";
                    //List<ProofRecord> feedingdata = MsMESHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0]
                    //.AsEnumerable()
                    //.Select(row => new ProofRecord{
                    //    MaterialID = row.Field<string>("MaterialID"),
                    //    Model = row.Field<string>("Model"),
                    //    PNo = row.Field<string>("PNo"),
                    //    Position = row.Field<string>("Position"),
                    //    FeedingDate = row.Field<DateTime>("FeedingDate")
                    //}).ToList();

                    //把查出来的PNo，Position放在新的集合，等下对比
                    List<Material> proofList = new List<Material>();
                    proofList = feedingdata.Select(row => new Material
                    {
                        PNo = row.PNo,
                        Position = row.Position
                    }).ToList();

                    //判断上料表是否成功，以下是找出对应型号，在对应生产线使用中的上料表(对比)
                    checkSame = IsSameMaterials(proofList, PartNum, linedata, voxcode, jobnum, out errmsg);
                    if (checkSame)
                    {
                        if (feedingdata.Count > 0)
                        {
                            bool isOk = false;
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.Append("insert into Engg.SmtProof_VoxLabel_MtlLotNum (GUID, TRIOLabelNo, LotNum, PartNum, PNo, Position, WorkShop, Line, Ip, InsertType, Isdelete, DateCode, AssociationDate, FeedingDate) values ");
                            foreach (ProofRecord f in feedingdata)
                            {
                                string strGuid = Guid.NewGuid().ToString();
                                sbSql.Append($"('{strGuid}','{voxcode}','{f.MaterialID}','{f.Model}','{f.PNo}','{f.Position}','{workShop}','{linedata}','{GetClentip()}','auto','0','{dateCode}',getdate(),'" + f.FeedingDate + "'),");
                                isOk = true;
                            }
                            strSql = sbSql.ToString();
                            if (!string.IsNullOrEmpty(strSql) && isOk)
                            {
                                int intResult = MsMESHelper.ExecuteNonQuery(strSql.TrimEnd(','));
                                if (intResult > 0)
                                    result = true;
                                else
                                    result = false;
                            }
                        }
                        else
                        {
                            //无上料记录
                            SaveFailLog("Missing data", voxcode, PartNum, $@"No feeding record,jobnum:{jobnum},ordernum:{ OrderNumber}");
                            errmsg = "No feeding record,jobnum:{jobnum},ordernum:{ OrderNumber}";
                            return result = false;
                        }
                    }
                    else
                    {
                        //物料数据不准确
                        SaveFailLog("Inaccurate material data", voxcode, PartNum, "Please check the materials:" + errmsg);
                        errmsg = $"Please check the materials:{errmsg}";
                        return result = false;
                    }
                }
                else
                {
                    //条码格式不正确
                    SaveFailLog("Missing data", voxcode, PartNum, "The barcode format is incorrect");
                    errmsg = $"[{voxcode}]The barcode format is incorrect";
                    return result = false;
                }
            }
            catch (Exception ex)
            {
                SaveFailLog("Program error" + ex.Message.ToString(), voxcode, string.Empty, ex.Message);
                errmsg = "Program error" + ex.Message.ToString();
                Logs.Write(ex);
            }
            return result;
        }

        /// <summary>
        /// 同步Erp的Vox子型号数据到MES
        /// </summary>
        public static void SyncPartNumData()
        {
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select identity(int, 1, 1) as item,0 as itemLv,erp.PartMtl.partnum as model, ");
                sbSql.Append("erp.PartMtl.MtlPartNum as partnum,erp.PartMtl.RevisionNum,qtyper,qtyper as 'totalQtyper', ");
                sbSql.Append("erp.PartMtl.uomcode into #voxa from erp.PartMtl with(nolock) ");
                sbSql.Append("inner join erp.PartRev with(nolock) on erp.PartMtl.Company = erp.PartRev.Company ");
                sbSql.Append("and erp.PartMtl.PartNum = erp.PartRev.PartNum ");
                sbSql.Append("and erp.PartMtl.RevisionNum = erp.PartRev.RevisionNum ");
                sbSql.Append("and erp.PartMtl.AltMethod = erp.PartRev.AltMethod ");
                sbSql.Append("inner join erp.ECORev with(nolock) on erp.PartMtl.Company = erp.ECORev.Company ");
                sbSql.Append("and erp.PartMtl.PartNum = erp.ECORev.PartNum ");
                sbSql.Append("and erp.PartMtl.RevisionNum = erp.ECORev.RevisionNum ");
                sbSql.Append("and erp.PartMtl.AltMethod = erp.ECORev.AltMethod ");
                sbSql.Append("where erp.PartMtl.Company = 1000 and erp.PartRev.partnum in( ");
                sbSql.Append("SELECT PartNum FROM [ERPLive].[Erp].[Part] with(nolock) ");
                sbSql.Append("where AnalysisCode = 'VOX' group by PartNum) ");
                sbSql.Append("and erp.PartRev.approved = 1 and erp.ECORev.checkedout = 0 ");
                sbSql.Append("and erp.PartRev.effectivedate =(select top 1 p.effectivedate ");
                sbSql.Append("from erp.PartRev p inner join erp.ECORev b on p.Company = b.Company ");
                sbSql.Append("and p.PartNum = b.PartNum and p.RevisionNum = b.RevisionNum ");
                sbSql.Append("and p.AltMethod = b.AltMethod where p.company = erp.PartRev.company ");
                sbSql.Append("and p.partnum = erp.PartRev.partnum and p.approved = 1 ");
                sbSql.Append("and b.checkedout = 0 and p.effectivedate <= getdate() ");
                sbSql.Append("order by p.effectivedate desc) declare @count int while (1 = 1) begin ");
                sbSql.Append("set @count = 0 insert into #voxa(itemlv,model,partnum,revisionnum,qtyper,totalQtyper,uomcode) ");
                sbSql.Append("select #voxa.item,#voxa.model ,PartMtl.MtlPartNum ,PartMtl.RevisionNum,PartMtl.qtyper, ");
                sbSql.Append("PartMtl.qtyper*totalQtyper,PartMtl.uomcode from #voxa ");
                sbSql.Append("inner join erp.PartMtl with(nolock) on #voxa.partnum=PartMtl.partnum ");
                sbSql.Append("inner join erp.PartRev with(nolock) on erp.PartMtl.Company = erp.PartRev.Company ");
                sbSql.Append("and erp.PartMtl.PartNum = erp.PartRev.PartNum ");
                sbSql.Append("and erp.PartMtl.RevisionNum = erp.PartRev.RevisionNum ");
                sbSql.Append("and erp.PartMtl.AltMethod = erp.PartRev.AltMethod ");
                sbSql.Append("inner join erp.ECORev with(nolock) on erp.PartMtl.Company = erp.ECORev.Company ");
                sbSql.Append("and erp.PartMtl.PartNum = erp.ECORev.PartNum ");
                sbSql.Append("and erp.PartMtl.RevisionNum = erp.ECORev.RevisionNum ");
                sbSql.Append("and erp.PartMtl.AltMethod = erp.ECORev.AltMethod ");
                sbSql.Append("where erp.PartMtl.Company = 1000 and erp.PartRev.approved = 1 and erp.ECORev.checkedout = 0 ");
                sbSql.Append("and erp.PartRev.effectivedate =(select top 1 p.effectivedate from ");
                sbSql.Append("erp.PartRev p inner join erp.ECORev b on p.Company = b.Company ");
                sbSql.Append("and p.PartNum = b.PartNum and p.RevisionNum = b.RevisionNum and p.AltMethod = b.AltMethod ");
                sbSql.Append("where p.company = PartRev.company and p.partnum = PartRev.partnum ");
                sbSql.Append("and p.approved = 1 and b.checkedout = 0 and p.effectivedate <= getdate() ");
                sbSql.Append("order by p.effectivedate desc) and #voxa.item  not in(select itemlv from #voxa) ");
                sbSql.Append("select @count += @@rowcount if(@count = 0) break end ");
                sbSql.Append("select Model,PartNum from #voxa where item  in(select itemlv from #voxa) ");
                sbSql.Append("group by model,partnum order by partnum asc drop table #voxa ");

                DataTable dtErp = MsENGGHelper.ExecuteDataset(CommandType.Text, sbSql.ToString()).Tables[0];
                if (dtErp.Rows.Count > 0)
                {
                    DataTable dtMes = MsMESHelper.ExecuteDataset(CommandType.Text, "select * from SmtProof_VoxLabel_MtlLotNum_Model").Tables[0];
                    if (dtErp.Rows.Count != dtMes.Rows.Count)
                    {
                        //做这个同步似乎没有意义，先搁置
                    }
                }
            }
            catch(Exception ex)
            {
                Logs.Write(ex);
            }
        }

        /// <summary>
        /// 判断是否为Datecode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDatecode(string str)
        {
            bool result = false;
            if (str.Length == 4)
            {
                int outInt;
                if (int.TryParse(str, out outInt))
                {
                    int intYear = Convert.ToInt32(str.Substring(0, 2));
                    int intWeek = Convert.ToInt32(str.Substring(2, 2));
                    var thisYear = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
                    if ((intYear > 17 && intYear <= thisYear) && intWeek > 0 && intWeek <= 53)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 物料信息
    /// </summary>
    public class Material
    {
        public string Position { get; set; }
        public string PNo { get; set; }
    }

    /// <summary>
    /// 防错上料记录
    /// </summary>
    public class ProofRecord
    {
        public string MaterialID { get; set; }
        public string Model { get; set; }
        public string PNo { get; set; }
        public string Position { get; set; }
        public DateTime FeedingDate { get; set; }
    }

    /// <summary>
    /// 海康发送过来的数据
    /// </summary>
    public class Dealdata
    {
        public string jobnum { get; set; }
        public string PartNum { get; set; }
        public string TRIOLabelNo { get; set; }
        public string DateCode { get; set; }
        public string FZjobnum { get; set; }
        public string Remark { get; set; }
        public string ScanResult { get; set; }
        public DateTime createtime { get; set; }
    }

    public static class ObjectExtensions
    {
        // 定义一个拓展方法
        public static string Dump<TObject>(this TObject @object)
        {
            return LitJson.JsonMapper.ToJson(@object);
        }
    }
}
