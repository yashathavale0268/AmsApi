using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{ 
    [DataContract]
    public class ServerInfoModel
    {

    [DataMember (Name = "ServInfoid")]
    public Int64 ServInfoid { get; set; }

    [DataMember(Name = "Pid")]
    public string PID { get; set; }

    [DataMember(Name = "EnvType")]
    public int EnvType { get; set; }

    [DataMember(Name = "BackupPath")]
    public string BackupPath { get; set; }

    [DataMember(Name = "Envlocation")]
    public int Envlocation { get; set; }

    [DataMember(Name = "Description")]
    public string Description { get; set; }

    [DataMember(Name = "Created_at")]
    public string Created_at { get; set; }
    }
}
