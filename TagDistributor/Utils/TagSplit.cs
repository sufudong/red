using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TagDistributor.DAL;

namespace TagDistributor.Utils
{
    public class TagSplit
    {
        public class RData
        {
            public Int64 Start { get; set; }

            public Int64 End { get; set; }
        }

        public static JObject selectData(string userName, long bid, long eid, long num, int flag)
        {
            if (1000 > bid)
            {
                bid = 1001;
            }
            if (-1 == eid)
            {
                eid = long.MaxValue -1;
            }
            if (bid > eid)
            {
                return new JObject
                {
                     { "msg", "failed bid and eid illegal" }
                };
            }

            eid++;

            //if (num < 1000)
            //{
            //    saveLog(bid, eid, num, "argumentIllegal", userName, "fail", null);
            //    return new JObject
            //    {
            //         { "bid", bid }, { "eId", eid }, { "err", "" }
            //    };
            //}

            //Debug.Assert(num > 1000, "the num of number must be greater than 1000");

            var dataContext = new TDContext();
            Array selectArray = null;
            if (0 == flag)
            {
                selectArray = (from m in dataContext.TagDistributs
                               where m.BeginID >= bid && m.BeginID <= eid
                               orderby m.BeginID ascending
                               select new RData { Start = m.BeginID, End = m.EndID }).ToArray<RData>();
            }
            else
            {
                selectArray = (from m in dataContext.TagDistributs
                               where m.BeginID >= bid && m.BeginID <= eid
                               orderby m.BeginID descending
                               select new RData { Start = m.BeginID, End = m.EndID }).ToArray<RData>();
            }


            if (0 == selectArray.Length)
            {
                // distribute
                if (0 == flag)
                {
                    return assignLabel(userName, bid, eid, num);
                }
                else
                {
                    return assignLabel(userName, eid - num, eid, num);
                }
            }
            else
            {
                long maxDiffValue = 0;
                long beginIndexNum = -1;
                bool assignSuccess = false;
                for (long i = 0; i < selectArray.Length; ++i)
                {
                    var tMov = (RData)selectArray.GetValue(i);
                    long startTag = -1;/// 前一个的结束，可分配的开始
                    long endTag = -1;/// 后一个的开始，可分配的结束
                    RData tMovNext = null;
                    if (0 == flag) /// 从前开始
                    {
                        startTag = tMov.End;
                        endTag = -1;
                        if (i == selectArray.Length - 1)
                        {
                            endTag = eid;
                        }
                        else
                        {
                            tMovNext = (RData)selectArray.GetValue(i + 1);
                            endTag = tMovNext.Start;
                        }
                    }
                    else //// 从后开始
                    {
                        startTag = tMov.End;
                        if (0 == i && tMov.End != eid)
                        {
                            endTag = eid;
                        }
                        else
                        {
                            if (0 != i)
                            {
                                tMovNext = (RData)selectArray.GetValue(i - 1);
                                endTag = tMovNext.Start;
                            }
                            else
                            {
                                endTag = eid;
                            }
                        }
                    }



                    long dValueOfRec = Math.Abs(endTag - startTag);
                    if (dValueOfRec >= num)
                    {
                        // success
                        assignSuccess = true;
                        if (0 == flag)
                        {
                            return assignLabel(userName, startTag, startTag + num, num);
                        }
                        else
                        {
                            return assignLabel(userName, endTag - num, endTag, num);
                        }
                    }
                    else
                    {
                        if (dValueOfRec > maxDiffValue)
                        {
                            maxDiffValue = dValueOfRec;
                            beginIndexNum = startTag;
                        }
                    }

                }
                if (!assignSuccess)
                {
                    // distributing maxDiffValue
                    return assignLabel(userName, beginIndexNum, beginIndexNum + maxDiffValue, num);
                }

            }
            Debug.Assert(false, "This is have an error, when run here");
            /// this is not run
            return null;
        }

        private static JObject assignLabel(string userName, long bid, long eid, long num)
        {
            if (bid == eid)
            {
                return new JObject { { "bId", bid }, { "eId", eid - 1 }, { "msg", "distribution of failed" } };
            }
            Debug.Assert(bid < eid);
            Models.TagDistribut tagDistribut = new Models.TagDistribut { BeginID = bid, EndID = eid - 1, Username = "dataCenter", DistributDate = DateTime.Now };


            var dataContext = new TDContext();
            dataContext.TagDistributs.Add(tagDistribut);
            dataContext.SaveChanges();

            saveLog(bid, eid, num, "Distribute", userName, "", dataContext);

            string msg = "unknow";
            long difValue = eid - bid;
            if (difValue == num)
            {
                msg = "Distribution of success";
            }
            else
            {
                if (difValue < num)
                {
                    msg = "Not fully allocated to succeed";
                }
            }

            return new JObject { { "bId", bid }, { "eId", eid - 1}, { "msg", msg } };
        }

        private static void saveLog(long bid, long eid, long expectNum, string distType, string userName, string success, TDContext dCtx)
        {
            if (null == dCtx)
            {
                dCtx = new TDContext();
            }
            JObject saveLogJson = new JObject{
                {"Type", distType}, {"BeginID", bid}, {"EndID", eid - 1}, {"success", success}, {"expectNum", expectNum}
            };
            Models.TagsLog logSaved = new Models.TagsLog
            {
                Info = saveLogJson.ToString(),
                Username = userName,
                DistributDate = DateTime.Now

            };
            dCtx.TagsLogs.Add(logSaved);
            dCtx.SaveChanges();
        }

        public static void rollback(string name, long bid, long num)
        {
            var ctx = new TDContext();

            var roll = ctx.TagDistributs.Single(c => c.BeginID == bid);
            ctx.TagDistributs.Remove(roll);
            ctx.SaveChanges();

            saveLog(bid, bid + num, num, "Rollback", name, "", ctx);
        }

    }
}