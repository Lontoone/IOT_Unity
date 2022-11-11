public interface IUdpDataReceiver
{
    //public byte[] datas { get; set; }
    public void DecodeData(byte[] _rawData);

}
