using SecureDongle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Security
{
    public class SecureDongleProvider
    {
        public static bool CheckHardKey()
        {
            try
            {
                SecureDonglecom SD = new SecureDonglecom();

                //Declare variable
                byte[] buffer = new byte[1024];
                object obbuffer = new object();
                ushort handle = 0;
                ushort p1 = 0;
                ushort p2 = 0;
                ushort p3 = 0;
                ushort p4 = 0;
                uint lp1 = 0;
                uint lp2 = 0;
                ulong ret = 1;


                p1 = 0x40B0;
                p2 = 0x8D28;
                p3 = 0xE602;     //advance password. Must set to 0 for end user application
                p4 = 0x2D29;     //advance password. Must set to 0 for end user application

                ret = SD.SecureDongle((ushort)SDCmd.SD_FIND, ref handle, ref lp1, ref lp2, ref p1, ref p2, ref p3, ref p4, ref obbuffer);

                if (ret != 0)
                {

                    return false;
                }
                else
                    return true;

            }
            catch
            {
            }
            return false;
        }

        enum SDCmd : ushort
        {
            SD_FIND = 1,//Find Dongle
            SD_FIND_NEXT = 2,//Find Next Dongle
            SD_OPEN = 3,//Open Dongle
            SD_CLOSE = 4,//Close Dongle
            SD_READ = 5,//Read Dongle
            SD_WRITE = 6,//Write Dongle
            SD_RANDOM = 7,//Generate Random Number
            SD_SEED = 8,//Generate Seed Code
            SD_WRITE_USERID = 9,//Write User ID
            SD_READ_USERID = 10,//Read User ID
            SD_SET_MODULE = 11,//Set Module
            SD_CHECK_MODULE = 12,//Check Module
            SD_WRITE_ARITHMETIC = 13,//Write Arithmetic
            SD_CALCULATE1 = 14,//Calculate 1
            SD_CALCULATE2 = 15,//Calculate 2
            SD_CALCULATE3 = 16,//Calculate 3
            SD_DECREASE = 17,//Decrease Module Unit
            SD_SET_COUNTER = 20,//set counter
            SD_GET_COUNTER = 21,//get counter
            SD_DEC_COUNTER = 22,
            SD_SET_TIMER = 23,//set timer
            SD_GET_TIMER = 24,//get timer
            SD_ADJUST_TIMER = 25,//adjust timer
            SD_SET_RSAKEY_N = 29,//write RSA N
            SD_SET_RSAKEY_D = 30,//write RSA D
            SD_UPDATE_GEN_HEADER = 31,//generate encrypted file header
            SD_UPDATE_GEN = 32,//create encrypted file content
            SD_UPDATE_CHECK = 33,//update cipher file
            SD_UPDATE = 34,//update cipher file
            SD_SET_DES_KEY = 41,//Set DES key
            SD_DES_ENC = 42,//DES encryption
            SD_DES_DEC = 43,//DES decryption
            SD_RSA_ENC = 44,//RSA encryption
            SD_RSA_DEC = 45,//RSA decryption
            SD_READ_EX = 46,//read dongle memory
            SD_WRITE_EX = 47,//write dongle memory
            SD_SET_COUNTER_EX = 0xA0,//set counter value type changed from WORD to DWORD
            SD_GET_COUNTER_EX = 0xA1,//get counter, value type changed from WORD to DWORD
            SD_SET_TIMER_EX = 0xA2,//set timer time value type changed from WORD to DWORD
            SD_GET_TIMER_EX = 0xA3,//get timer time value type changed from WORD to DWORD
            SD_ADJUST_TIMER_EX = 0xA4,//adjust timer, time value type changed from WORD to DWORD
            SD_UPDATE_GEN_HEADER_EX = 0xA5,//generate update file header specialize in updating RSA key pair
            SD_UPDATE_GEN_EX = 0xA6,//generate update file content specialize in updating RSA key pair
            SD_UPDATE_CHECK_EX = 0xA7,//update file checking specialize in updating RSA key pair
            SD_UPDATE_EX = 0xA8,//update cipher file specialize in updating RSA key pair
            SD_SET_UPDATE_KEY = 0xA9,//set update RSA key pair
            SD_ADD_UPDATE_HEADER = 0xAA,//fill head of authorization file
            SD_ADD_UPDATE_CONTENT = 0xAB,//fill content of authorization file
            SD_GET_TIME_DWORD = 0xAC,//get value(DWORD type) based on 2006.1.1.0.0.0
            SD_VERSION = 100,//get COS Version
        };
    }
}
