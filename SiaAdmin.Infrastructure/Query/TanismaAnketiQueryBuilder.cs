using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Domain.Entities.ReportModel;

namespace SiaAdmin.Infrastructure.Query
{
    public class TanismaAnketiQueryBuilder : IQueryTypeBuilder
    {
        public bool CanHandle(ReportType reportType)
        {
            return reportType == ReportType.Custom;
        }

        public string BuildQuery(Report report, string dateRange, string additionalParams)
        {
            if (report.Id != "601")
            {
                return null;
            }

            var query = @"
                SELECT 
sys_RespNum,
dateadd(minute, datepart(TZoffset, sysdatetimeoffset()), [dbo].Unixtime2Datetime(sys_StartTimeStamp)) as BaslangicZamani,dateadd(minute, datepart(TZoffset, sysdatetimeoffset()), [dbo].Unixtime2Datetime(sys_EndTimeStamp)) as BitisZamani,
sys_respstatus,
sys_DispositionCode,
sys_UserAgent,
sys_OperatingSystem, 
sys_Browser,
sys_IPAddress,
g,
--[ACIKYAS],
[DOGUMYILI],
--CINSIYETKod,
Cinsiyet,
--ILKod,
Il,
HHR,
--EgitimGKKod,
EgitimGK,
--EgitimHRKod,
EgitimHR,
--EmekliKod,
Emekli,
AMeslekGK,
AMeslekHR,
--YMeslekGKKod,
YMeslekGK,
--YMeslekHRKod,
YMeslekHR,
--YMeslekDetayGKKod,
YMeslekDetayGK,
--YMeslekDetayHRKod,
YMeslekDetayHR,
--YSESKod,
YSES

FROM OPENQUERY([212.115.43.204], 'SELECT 
a.sys_RespNum,
sys_StartTimeStamp,sys_EndTimeStamp,
sys_respstatus,sys_DispositionCode
,[sys_UserAgent],[sys_OperatingSystem],[sys_Browser],[sys_IPAddress],[g]
,[DOGUMYILI]
--,[CINSIYET] as [CINSIYETKod]
,Cinsiyet =  
CASE [CINSIYET]  
WHEN ''1'' THEN ''Kadın''  
WHEN ''2'' THEN ''Erkek''  
ELSE ''#HATA#''  
END
--,[IL] as [ILKod]
,Il =
CASE[IL]
When ''1'' Then ''Adana''
When ''2'' Then ''Adıyaman''
When ''3'' Then ''Afyon''
When ''4'' Then ''Ağrı''
When ''5'' Then ''Amasya''
When ''6'' Then ''Ankara''
When ''7'' Then ''Antalya''
When ''8'' Then ''Artvin''
When ''9'' Then ''Aydın''
When ''10'' Then ''Balıkesir''
When ''11'' Then ''Bilecik''
When ''12'' Then ''Bingöl''
When ''13'' Then ''Bitlis''
When ''14'' Then ''Bolu''
When ''15'' Then ''Burdur''
When ''16'' Then ''Bursa''
When ''17'' Then ''Çanakkale''
When ''18'' Then ''Çankırı''
When ''19'' Then ''Çorum''
When ''20'' Then ''Denizli''
When ''21'' Then ''Diyarbakır''
When ''22'' Then ''Edirne''
When ''23'' Then ''Elazığ''
When ''24'' Then ''Erzincan''
When ''25'' Then ''Erzurum''
When ''26'' Then ''Eskişehir''
When ''27'' Then ''Gaziantep''
When ''28'' Then ''Giresun''
When ''29'' Then ''Gümüşhane''
When ''30'' Then ''Hakkâri''
When ''31'' Then ''Hatay''
When ''32'' Then ''Isparta''
When ''33'' Then ''Mersin''
When ''34'' Then ''İstanbul''
When ''35'' Then ''İzmir''
When ''36'' Then ''Kars''
When ''37'' Then ''Kastamonu''
When ''38'' Then ''Kayseri''
When ''39'' Then ''Kırklareli''
When ''40'' Then ''Kırşehir''
When ''41'' Then ''Kocaeli''
When ''42'' Then ''Konya''
When ''43'' Then ''Kütahya''
When ''44'' Then ''Malatya''
When ''45'' Then ''Manisa''
When ''46'' Then ''Kahramanmaraş''
When ''47'' Then ''Mardin''
When ''48'' Then ''Muğla''
When ''49'' Then ''Muş''
When ''50'' Then ''Nevşehir''
When ''51'' Then ''Niğde''
When ''52'' Then ''Ordu''
When ''53'' Then ''Rize''
When ''54'' Then ''Sakarya''
When ''55'' Then ''Samsun''
When ''56'' Then ''Siirt''
When ''57'' Then ''Sinop''
When ''58'' Then ''Sivas''
When ''59'' Then ''Tekirdağ''
When ''60'' Then ''Tokat''
When ''61'' Then ''Trabzon''
When ''62'' Then ''Tunceli''
When ''63'' Then ''Şanlıurfa''
When ''64'' Then ''Uşak''
When ''65'' Then ''Van''
When ''66'' Then ''Yozgat''
When ''67'' Then ''Zonguldak''
When ''68'' Then ''Aksaray''
When ''69'' Then ''Bayburt''
When ''70'' Then ''Karaman''
When ''71'' Then ''Kırıkkale''
When ''72'' Then ''Batman''
When ''73'' Then ''Şırnak''
When ''74'' Then ''Bartın''
When ''75'' Then ''Ardahan''
When ''76'' Then ''Iğdır''
When ''77'' Then ''Yalova''
When ''78'' Then ''Karabük''
When ''79'' Then ''Kilis''
When ''80'' Then ''Osmaniye''
When ''81'' Then ''Düzce''
When ''82'' Then ''Diğer''
ELSE ''#HATA#''  
END
--,[EgitimGK] as [EgitimGKKod]
,HHR = CASE [HHR]
When ''1'' Then ''Benim''
When ''2'' Then ''Bir Başkası''
ELSE ''#HATA#''  
END
,EgitimGK =
CASE [EgitimGK]
When ''1'' Then ''Yüksek lisans-doktora-tıpta uzmanlık''
When ''2'' Then ''Üniversite normal''
When ''3'' Then ''Üniversite açık''
When ''4'' Then ''Yüksekokul''
When ''5'' Then ''Meslek lisesi''
When ''6'' Then ''Düz lise''
When ''7'' Then ''Ortaokul''
When ''8'' Then ''İlkokul''
When ''9'' Then ''Eğitimsiz''
ELSE ''#HATA#''  
END
--,[EgitimHR] as [EgitimHRKod]
,EgitimHR =
CASE [EgitimHR]
When ''1'' Then ''Yüksek lisans-doktora-tıpta uzmanlık''
When ''2'' Then ''Üniversite normal''
When ''3'' Then ''Üniversite açık''
When ''4'' Then ''Yüksekokul''
When ''5'' Then ''Meslek lisesi''
When ''6'' Then ''Düz lise''
When ''7'' Then ''Ortaokul''
When ''8'' Then ''İlkokul''
When ''9'' Then ''Eğitimsiz''
ELSE ''#YOK#''  
END
--,[Emekli] as [EmekliKod]
,Emekli = 
CASE [Emekli]
When ''1'' Then ''Emekli değil''
When ''2'' Then ''Emekli - Çalışıyor''
When ''3'' Then ''Emekli - Çalışmıyor''
ELSE ''#HATA#''  
END
      ,[AMeslekGK]
      ,[AMeslekHR]

--,[YMeslekGK] as [YMeslekGKKod]
,YMeslekGK = ''#YOK#''
/*
CASE [YMeslekGK]
When ''1'' Then ''Gelir Getiren Bir İşi Yok, Çalışmıyor''
When ''2'' Then ''Ücretli/ Maaşlı Çalışıyor''
When ''3'' Then ''Kendi Hesabına Çalışıyor/ Serbest Meslek/ Nitelikli Uzman''
ELSE ''#HATA#''  
END
*/
--,[YMeslekHR] as [YMeslekHRKod]
,YMeslekHR = ''#YOK#''
/*
CASE [YMeslekHR]
When ''1'' Then ''Gelir Getiren Bir İşi Yok, Çalışmıyor''
When ''2'' Then ''Ücretli/ Maaşlı Çalışıyor''
When ''3'' Then ''Kendi Hesabına Çalışıyor/ Serbest Meslek/ Nitelikli Uzman''
ELSE ''#HATA#''  
END
*/
--,[YMeslekDetayGK] as [YMeslekDetayGKKod]
,YMeslekDetayGK = 
CASE [YMeslekDetayGK]
When ''1'' Then ''İşsiz – Şuan çalışmıyor – ek gelir yok, yardım alıyor''
When ''2'' Then ''İşsiz – Şuan çalışmıyor – düzenli ek gelir var''
When ''3'' Then ''Ev kadını – ek gelir yok, yardım alıyor''
When ''4'' Then ''Ev kadını – düzenli ek gelir var''
When ''5'' Then ''Öğrenci (gelir getiren bir işi olmayan''
When ''6'' Then ''Işçi/hizmetli - parça başı işi olan (yevmiye)''
When ''7'' Then ''Işçi/hizmetli - düzenli işi olan (maaş)''
When ''8'' Then ''Ustabaşı/kalfa - kendine bağlı işçi çalışan''
When ''9'' Then ''Yönetici olmayan memur / teknik eleman / uzman vs''
When ''10'' Then ''Yönetici (1-5 çalışanı olan)''
When ''11'' Then ''Yönetici (6-10 çalışanı olan)''
When ''12'' Then ''Yönetici (11-20 çalışanı olan)''
When ''13'' Then ''Yönetici (20`den fazla çalışanı olan)''
When ''14'' Then ''Ordu mensubu (uzman er, astsubay, subay)''
When ''15'' Then ''Ücretli Kıdemli Nitelikli Uzman (Avukat, Doktor, Mimar, Mühendis, Akademisyen vs)''
When ''16'' Then ''Çiftçi (kendi başına/ailesiyle çalışan)''
When ''17'' Then ''Seyyar - Kendi isi (free lance dahil), dükkanda hizmet vermiyor''
When ''18'' Then ''Tek başına çalışan, dükkan sahibi, esnaf (taksi şoförü dahil)''
When ''19'' Then ''1-5 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''20'' Then ''6-10 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''21'' Then ''11-20 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''22'' Then ''20`den fazla çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''23'' Then ''Serbest nitelikli uzman (Avukat, Mühendis, Mali Müşavir, Doktor, Eczacı vs)''
ELSE ''#HATA#''  
END

--,[YMeslekDetayHR] as [YMeslekDetayHRKod]
,YMeslekDetayHR = 
CASE [YMeslekDetayHR]
When ''1'' Then ''İşsiz – Şuan çalışmıyor – ek gelir yok, yardım alıyor''
When ''2'' Then ''İşsiz – Şuan çalışmıyor – düzenli ek gelir var''
When ''3'' Then ''Ev kadını – ek gelir yok, yardım alıyor''
When ''4'' Then ''Ev kadını – düzenli ek gelir var''
When ''5'' Then ''Öğrenci (gelir getiren bir işi olmayan''
When ''6'' Then ''Işçi/hizmetli - parça başı işi olan (yevmiye)''
When ''7'' Then ''Işçi/hizmetli - düzenli işi olan (maaş)''
When ''8'' Then ''Ustabaşı/kalfa - kendine bağlı işçi çalışan''
When ''9'' Then ''Yönetici olmayan memur / teknik eleman / uzman vs''
When ''10'' Then ''Yönetici (1-5 çalışanı olan)''
When ''11'' Then ''Yönetici (6-10 çalışanı olan)''
When ''12'' Then ''Yönetici (11-20 çalışanı olan)''
When ''13'' Then ''Yönetici (20`den fazla çalışanı olan)''
When ''14'' Then ''Ordu mensubu (uzman er, astsubay, subay)''
When ''15'' Then ''Ücretli Kıdemli Nitelikli Uzman (Avukat, Doktor, Mimar, Mühendis, Akademisyen vs)''
When ''16'' Then ''Çiftçi (kendi başına/ailesiyle çalışan)''
When ''17'' Then ''Seyyar - Kendi isi (free lance dahil), dükkanda hizmet vermiyor''
When ''18'' Then ''Tek başına çalışan, dükkan sahibi, esnaf (taksi şoförü dahil)''
When ''19'' Then ''1-5 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''20'' Then ''6-10 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''21'' Then ''11-20 çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''22'' Then ''20`den fazla çalışanlı isyeri sahibi (Tic, Tarım, İmalat, Hizmet)''
When ''23'' Then ''Serbest nitelikli uzman (Avukat, Mühendis, Mali Müşavir, Doktor, Eczacı vs)''
ELSE ''#BOŞ#''  
END

--,[YSES] as [YSESKod]
,YSES = 
CASE [YSES]
When ''1'' Then ''A''
When ''2'' Then ''B''
When ''3'' Then ''C1''
When ''4'' Then ''C2''
When ''5'' Then ''D''
When ''6'' Then ''E''
ELSE ''#HATA#''  
END

FROM [SSISurveys2016].[dbo].[PFGrup4_data1] as a WITH (NOLOCK)

where sys_respstatus = 5
order by sys_RespNum')";

            return query;
        }

    }
}
