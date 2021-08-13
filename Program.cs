using System;
using System.Collections.Generic;
using System.Linq;
namespace DiePoker
{
    class Program
    {
        static int cnt = 0;//计数器，函数调用次数
        static HashSet<string> deadSet = new HashSet<string>();//因为涉及一些重复判断，所以使用一个set记录必输的情况，减少迭代次数
        static void Main(string[] args)
        {
            var currList = new List<int> { 3, 5, 7 };
            var res = decide(currList);
            Console.WriteLine(res);
            Console.WriteLine(cnt);
            Console.WriteLine(string.Join(",", deadSet));
            Console.ReadKey();
        }
        /// <summary>
        /// 局势转化为局势码
        /// </summary>
        /// <param name="currList"></param>
        /// <returns></returns>
        static string genDeadNum(List<int> currList) {
            return string.Join("-",currList.OrderByDescending(o=>o));
        }
        /// <summary>
        /// 计算成败
        /// </summary>
        /// <param name="currList"></param>
        /// <returns></returns>
        static int decide(List<int> currList) {
            List<int> oldList = currList;
            for (int pileIdx = 0; pileIdx <  Enumerable.Range(0, currList.Count).Count(); pileIdx ++) {
                //如果本身没有火柴，略过
                if (currList[pileIdx] == 0)
                {
                    continue;
                }
                else {
                    var temp = currList[pileIdx];
                    //选择拿走之后剩余的数量
                    for (var num = 0; num < Enumerable.Range(0, temp).Count(); num++) {
                        currList[pileIdx] = num;
                        //如果都拿没了，说明我输了，我们先略过
                        if (currList.Sum() == 0)
                        {
                            continue;
                        }
                        else   //还有火柴看对手处境
                        {
                            var testNum = genDeadNum(currList);
                            //如果对应局势是必输，或者我们迭代后发现结果为0，对手必输，我们必胜，返回 1
                            if (deadSet.Contains(testNum) || decide(currList) == 0) {
                                currList[pileIdx] = temp;
                                cnt++;
                                return 1;
                            }
                        }
                    }
                    //遍历完取这堆石子的可能性，还原这堆石子的初始状况
                    currList[pileIdx] = temp;
                }
            }
            //生成必将失败的局势码加到死亡集合中
            var deadNum = genDeadNum(currList);
            cnt++;
            deadSet.Add(deadNum);
            return 0;
        }
    }
}
