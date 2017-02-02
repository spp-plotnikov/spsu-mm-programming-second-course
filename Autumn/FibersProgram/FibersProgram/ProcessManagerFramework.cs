using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibersProgram
{
    public static class ProcessManagerFramework
    {
        public static List<Fiber> listFibers = new List<Fiber>();
        private static int curFiberAssist = 0;
        private static int curFiber = 0;

        #region  This code for priority swich
        public static bool priority;
        private static Random rand = new Random();
        private static Fiber someFiber;
        #endregion

        public static void Switch(bool fiberFinished)
        {

            //Console.WriteLine(fiberFinished);
            if (fiberFinished)
            {
                //Console.WriteLine("listFibers.Count = " + listFibers.Count);
                //Console.WriteLine("curFiber = " + curFiber);
                Console.WriteLine(listFibers[curFiber].Id + " was removed");
                #region This code for priority swich
                if (priority)
                {
                    someFiber = listFibers[curFiber];
                    for(int i=0; i<listFibers.Count;)
                    {
                        if (listFibers[i] == someFiber)
                            listFibers.RemoveAt(i);
                        else
                            i++;
                    }
                }
                else
                #endregion  
                    listFibers.Remove(listFibers[curFiber]);

                if (listFibers.Count == 0)
                {
                    try
                    {
                        Fiber.Switch(Fiber.PrimaryId);
                    }
                    catch
                    {
                    }
                    return;
                }
                else
                {
                    curFiber = curFiberAssist;
                    if (curFiberAssist >= listFibers.Count)
                        curFiber = (curFiber + 1) % listFibers.Count;

                    #region  This code for priority swich
                    if (priority)
                        curFiber = rand.Next(0, listFibers.Count - 1);
                    #endregion

                    //Console.WriteLine("curFiber = " + curFiber);
                    //Console.WriteLine("listFibers.Count = " + listFibers.Count);
                    curFiberAssist = (curFiber + 1) % listFibers.Count;

                    
                    Fiber.Switch(listFibers[curFiber].Id);

                }
            }
            else
            {
                curFiber = curFiberAssist;
                curFiberAssist = (curFiber + 1) % listFibers.Count;

                #region  This code for priority swich
                if (priority)
                    curFiber = rand.Next(0, listFibers.Count - 1);
                #endregion

                //Console.WriteLine(curFiber);
                Fiber.Switch(listFibers[curFiber].Id);
            }
        }
    }
}
