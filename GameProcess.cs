using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SWGPatcher
{
    public class GameProcess
    {
        String executable;
        public GameProcess(String executable)
        {
            this.executable = executable;
        }

        public int Run()
        {
            if (File.Exists(executable) == false) {
                throw new Exception("Game executable could not be found. Please check the folder");
            }
            

            return 0;
        }
    }
}
