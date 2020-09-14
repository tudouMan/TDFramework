using System.IO;
using System.Net;
using System;

namespace Framework
{
	public class NetStream
	{
		private MemoryStream stream;
		private BinaryReader reader;
		private BinaryWriter writer;

		public NetStream(byte[] buffer = null)
		{
			if (buffer == null)
			{
				this.stream = new MemoryStream();
			}
			else
			{
				this.stream = new MemoryStream(buffer);
			}

			this.reader = new BinaryReader(this.stream);
			this.writer = new BinaryWriter(this.stream);
		}

		public void Close()
		{
			this.stream.Close();
			this.reader.Close();
			this.writer.Close();
		}

		public long ReadInt64()
		{
			return IPAddress.HostToNetworkOrder(this.reader.ReadInt64());
		}

		public int ReadInt32()
		{
			return IPAddress.HostToNetworkOrder(this.reader.ReadInt32());
		}

		public int ReadInt16()
		{
			return IPAddress.HostToNetworkOrder(this.reader.ReadInt16());
		}

		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		public string ReadString8()
		{
			return System.Text.Encoding.UTF8.GetString
				   (
				   		this.reader.ReadBytes(ReadByte())
				   );
		}

		public string ReadString16()
		{
			return System.Text.Encoding.UTF8.GetString
				   (
				   		this.reader.ReadBytes(ReadInt16())
				   );
		}

		public long Seek(long offset)
		{
			return this.stream.Seek(offset, SeekOrigin.Begin);
		}

		// -------------------------------------------------------------------------------

		public void WriteByte(byte value)
		{
			this.writer.Write(value);
		}


		public void WriteInt16(short value)
		{
			this.writer.Write
			(
				BitConverter.GetBytes
				(
					IPAddress.HostToNetworkOrder(value)
				)
			);
		}

		public void WriteInt32(int value)
		{
			this.writer.Write
			(
				BitConverter.GetBytes
				(
					IPAddress.HostToNetworkOrder(value)
				)
			);
		}

		public void WriteInt64(long value)
		{
			this.writer.Write
			(
				BitConverter.GetBytes
				(
					IPAddress.HostToNetworkOrder(value)
				)
			);
		}

		public void WriteString8(string value)
		{
			WriteByte
			(
				(byte)value.Length
			);


			this.writer.Write
			(
				System.Text.Encoding.UTF8.GetBytes(value)
			);
		}


		public void WriteString16(string value)
		{
			WriteInt16
			(
				(short)value.Length
			);


			this.writer.Write
			(
				System.Text.Encoding.UTF8.GetBytes(value)
			);
		}

		public byte[] GetBuffer()
		{
			return this.stream.ToArray();
		}

		public int GetLength()
		{
			return (int)this.stream.Length;
		}
	}
}