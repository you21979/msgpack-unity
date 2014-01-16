using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using MsgPack;
class App {
    public class MemberData
    {
        public ulong member_id;
        public uint char_id;
        public string name;
        public MemberData(){
            member_id = 100000000000;
            char_id = 100000000;
            name = "testtesttest";
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
    static int Main (string [] args)
    {
        CompiledPacker packer = new CompiledPacker();
        var club = new Club();
        var x = packer.Pack<Club>( club );
        //Console.Write(x.Length);
        var y = packer.Unpack<Club>( x );
        Console.Write(y.set_year);
        Console.Write(y.member.name);
        return 0;
    }
}
