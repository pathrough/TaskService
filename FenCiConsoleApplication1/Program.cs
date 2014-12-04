using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenCiConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "。不会显得唐突而且不会引起猜疑。如果只是问一句“在吗”，然后等着对方回复就太被动了，试想一下，如果自己收到许久不见的朋友发来“在吗”，第一反应是“他找我会有什么事”，然后就会闪过各种猜想，因为许久不见，这样的招呼就会显得唐突而且冰冷，所以一般的猜想会是对方遇到了什么麻烦需要帮忙，甚至是借钱，这个时候，如果曾经关系不错并且还想维持关系的，就会回“在呀”或者“怎么了”，如果曾经关系一般今后不想再见的，那就干脆会选择沉默不回应来中断关系。但是如果是发了一张照片或者直接叙述为什么会想到对方，接收信息的一方一开始就不会进行各种猜想，从而打消疑虑，甚至敞开心扉跟你叙旧，继而畅想未来。。。之类的。";
            List<string> candidateWords = new List<string>();
            string[] dict = File.ReadAllLines("sDict.txt");
            int reachIndex = 0;
            do
            {                
                for (int i = reachIndex; i < input.Length; i++)
                {
                    string temWord = input.Substring(reachIndex, i - reachIndex + 1);
                    if (dict.Where(d => d.StartsWith(temWord)).Count() > 0)
                    {
                        candidateWords.Add(temWord);
                    }
                    else if (reachIndex == i)
                    {
                        //防止死循环
                        reachIndex = i+1;
                        break;
                    }
                    else
                    {
                        reachIndex = i;
                        break;
                    }
                }
            }
            while (reachIndex < input.Length);
        }
    }
}
