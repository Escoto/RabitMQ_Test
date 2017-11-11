using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabitMQ_NET_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MyRabbitClient.TestRabbit();

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}
