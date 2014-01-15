using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using MsgPack;

interface Serializer                                                                                                                                                                                                                        
{
    byte[] Serialize(IDictionary<string, object> param);
    IDictionary<string, object> Deserialize(byte[] data);
}
public class MsgPackSerializer : Serializer                                                                                                                                                                                                 
{                                                                                                                                                                                                                                           
    BoxingPacker packer = new BoxingPacker();
    public MsgPackSerializer(){
    }
    public byte[] Serialize(IDictionary<string, object> param){
        return packer.Pack( param );
    }
    public IDictionary<string, object> Deserialize(byte[] data) {
        IDictionary<string, object> dict = packer.Unpack( data ) as IDictionary<string, object>;
        return dict;
    }
}
class App {
    public class MemberData
    {
        public uint member_id;
        public uint char_id;
        public string name;
        public MemberData(){
            member_id = 0;
            char_id = 0;
            name = "";
        }
    }
    public class Club
    {
        public MemberData member;
        public uint set_year;
        public Club(){
            member = new MemberData();
            set_year = 0;
        }
    }
    public static MemberData MemberDataDeserialize(IDictionary p){
        var member = new MemberData();
        member.member_id = PrimitiveDeserialize<uint>(p["member_id"]);
        member.char_id = PrimitiveDeserialize<uint>(p["char_id"]);
        member.name = PrimitiveDeserialize<string>(p["name"]);
        return member;
    }

    public static Type PrimitiveDeserialize<Type>(object p){
        return (Type)p;
    }

    public static Club ClubDeserialize(IDictionary p)
    {
        var param = new Club();
        param.member = MemberDataDeserialize((IDictionary)p["member"]);
        param.set_year = PrimitiveDeserialize<uint>(p["set_year"]);
        return param;
    }

    public static IDictionary MemberDataSerialize(MemberData p)
    {
        var d = new Dictionary<string,object>();
        d.Add("member_id", p.member_id);
        d.Add("char_id", p.char_id); 
        d.Add("name", p.name); 
        return d;
    }
    public static IDictionary ClubSerialize(Club p)
    {
        var d = new Dictionary<string,object>();
        d.Add("member", MemberDataSerialize(p.member));
        d.Add("set_year", p.set_year);
        return d;
    }

    static int Main (string [] args)
    {
        var param = new Club();
        param.set_year = 1979;
        param.member.char_id = 1;
        param.member.name = "aaaiiii";

        var dic = ClubSerialize(param);
        var s = ClubDeserialize(dic);
        Console.Write(s.set_year);
        Console.Write("\n");
        Console.Write(s.member.name);
        Console.Write("\n");

        var serializer = new BoxingPacker ();
        var bin = serializer.Pack(dic);
        var data = serializer.Unpack(bin);
        Console.Write(((IDictionary)data)["set_year"]);
        var a = (IDictionary)data;
        var ss = ClubDeserialize(a);
        Console.Write(ss.set_year);
        

        return 0;
    }
}
