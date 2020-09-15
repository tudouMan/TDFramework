using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct TestFrameProto
{
    public ushort ProtoCode => 30000;

    public int ID;

    public int FrameIndex;

    public int mDirect;   //0 代表Left 1 Right

    public byte[] ToArray()
    {
        using (ByteMemoryStream ms = new ByteMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(ID);
            ms.WriteInt(FrameIndex);
            ms.WriteInt(mDirect);
            return ms.ToArray();
        }
    }

    public static TestFrameProto GetProto(byte[] buffer)
    {
        TestFrameProto proto = new TestFrameProto();
        using (ByteMemoryStream ms = new ByteMemoryStream(buffer))
        {
            proto.ID = ms.ReadInt();
            proto.FrameIndex = ms.ReadInt();
            proto.mDirect = ms.ReadInt();
        }
        return proto;
    }
}

