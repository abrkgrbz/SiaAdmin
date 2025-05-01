using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.Sms
{
    public  interface ISmsService
    {
        public string Post2SmsOrigin(string Data2Post);
        public double GetCredit();
        public string GetSmsOriginStatusText(int StatusId);
        public string ReturnSmsCodeErrorCode(string retval);

        public double SendSmsOneToMany( string Recipients,string randomNumber);

        public string SendDataIYS(string regionCode,string msisdn,string internalGuid);
    }
}
