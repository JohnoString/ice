// Copyright (c) ZeroC, Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test;

namespace ZeroC.Ice.Test.Slicing.Objects
{
    public sealed class TestIntfAsync : ITestIntfAsync
    {
        public ValueTask ShutdownAsync(Current current)
        {
            current.Adapter.Communicator.ShutdownAsync();
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<AnyClass?> SBaseAsObjectAsync(Current current) =>
            new ValueTask<AnyClass?>(new SBase("SBase.sb"));

        public ValueTask<SBase?> SBaseAsSBaseAsync(Current current) =>
            new ValueTask<SBase?>(new SBase("SBase.sb"));

        public ValueTask<SBase?> SBSKnownDerivedAsSBaseAsync(Current current) =>
            new ValueTask<SBase?>(new SBSKnownDerived("SBSKnownDerived.sb", "SBSKnownDerived.sbskd"));

        public ValueTask<SBSKnownDerived?> SBSKnownDerivedAsSBSKnownDerivedAsync(Current current) =>
            new ValueTask<SBSKnownDerived?>(new SBSKnownDerived("SBSKnownDerived.sb", "SBSKnownDerived.sbskd"));

        public ValueTask<SBase?> SBSUnknownDerivedAsSBaseAsync(Current current) =>
            new ValueTask<SBase?>(new SBSUnknownDerived("SBSUnknownDerived.sb", "SBSUnknownDerived.sbsud"));

        public ValueTask CUnknownAsSBaseAsync(SBase? cUnknown, Current current)
        {
            if (cUnknown!.Sb != "CUnknown.sb")
            {
                throw new Exception();
            }
            return new ValueTask();
        }

        public ValueTask<SBase?> SBSUnknownDerivedAsSBaseCompactAsync(Current current) =>
            new ValueTask<SBase?>(new SBSUnknownDerived("SBSUnknownDerived.sb", "SBSUnknownDerived.sbsud"));

        public ValueTask<AnyClass?> SUnknownAsObjectAsync(Current current)
        {
            var su = new SUnknown("SUnknown.su", null);
            su.Cycle = su;
            return new ValueTask<AnyClass?>(su);
        }

        public ValueTask CheckSUnknownAsync(AnyClass? obj, Current current)
        {
            TestHelper.Assert(obj != null);
            var su = (SUnknown)obj;
            TestHelper.Assert(su.Su.Equals("SUnknown.su"));
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<B?> OneElementCycleAsync(Current current)
        {
            var b = new B();
            b.Sb = "B1.sb";
            b.Pb = b;
            return new ValueTask<B?>(b);
        }

        public ValueTask<B?> TwoElementCycleAsync(Current current)
        {
            var b1 = new B();
            b1.Sb = "B1.sb";
            var b2 = new B();
            b2.Sb = "B2.sb";
            b2.Pb = b1;
            b1.Pb = b2;
            return new ValueTask<B?>(b1);
        }

        public ValueTask<B?> D1AsBAsync(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return new ValueTask<B?>(d1);
        }

        public ValueTask<D1?> D1AsD1Async(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return new ValueTask<D1?>(d1);
        }

        public ValueTask<B?> D2AsBAsync(Current current)
        {
            var d2 = new D2();
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            var d1 = new D1();
            d1.Pb = d2;
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            d1.Pd1 = d2;
            d2.Pb = d1;
            d2.Pd2 = d1;
            return new ValueTask<B?>(d2);
        }

        public ValueTask<(B?, B?)> ParamTest1Async(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return MakeValueTask(((B?)d1, (B?)d2));
        }

        public ValueTask<(B?, B?)> ParamTest2Async(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return MakeValueTask(((B?)d2, (B?)d1));
        }

        public ValueTask<(B?, B?, B?)> ParamTest3Async(Current current)
        {
            var d2 = new D2();
            d2.Sb = "D2.sb (p1 1)";
            d2.Pb = null;
            d2.Sd2 = "D2.sd2 (p1 1)";

            var d1 = new D1();
            d1.Sb = "D1.sb (p1 2)";
            d1.Pb = null;
            d1.Sd1 = "D1.sd2 (p1 2)";
            d1.Pd1 = null;
            d2.Pd2 = d1;

            var d4 = new D2();
            d4.Sb = "D2.sb (p2 1)";
            d4.Pb = null;
            d4.Sd2 = "D2.sd2 (p2 1)";

            var d3 = new D1();
            d3.Sb = "D1.sb (p2 2)";
            d3.Pb = null;
            d3.Sd1 = "D1.sd2 (p2 2)";
            d3.Pd1 = null;
            d4.Pd2 = d3;

            return MakeValueTask(((B?)d3, (B?)d2, (B?)d4));
        }

        public ValueTask<(B?, B?)> ParamTest4Async(Current current)
        {
            var d4 = new D4();
            d4.Sb = "D4.sb (1)";
            d4.Pb = null;
            d4.P1 = new B();
            d4.P1.Sb = "B.sb (1)";
            d4.P2 = new B();
            d4.P2.Sb = "B.sb (2)";
            return MakeValueTask(((B?)d4.P2, (B?)d4));
        }

        public ValueTask<(B?, B?, B?)> ReturnTest1Async(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return MakeValueTask(((B?)d2, (B?)d2, (B?)d1));
        }

        public ValueTask<(B?, B?, B?)> ReturnTest2Async(Current current)
        {
            var d1 = new D1();
            d1.Sb = "D1.sb";
            d1.Sd1 = "D1.sd1";
            var d2 = new D2();
            d2.Pb = d1;
            d2.Sb = "D2.sb";
            d2.Sd2 = "D2.sd2";
            d2.Pd2 = d1;
            d1.Pb = d2;
            d1.Pd1 = d2;
            return new ValueTask<(B?, B?, B?)>((d1, d1, d2));
        }

        public ValueTask<B?> ReturnTest3Async(B? p1, B? p2, Current current) => MakeValueTask(p1);

        public ValueTask<SS3> SequenceTestAsync(SS1? p1, SS2? p2, Current current) => MakeValueTask(new SS3(p1, p2));

        public ValueTask<(IReadOnlyDictionary<int, B?>, IReadOnlyDictionary<int, B?>)> DictionaryTestAsync(
            Dictionary<int, B?> bin,
            Current current)
        {
            var bout = new Dictionary<int, B?>();
            int i;
            for (i = 0; i < 10; ++i)
            {
                B? b = bin[i];
                TestHelper.Assert(b != null);
                var d2 = new D2();
                d2.Sb = b.Sb;
                d2.Pb = b.Pb;
                d2.Sd2 = "D2";
                d2.Pd2 = d2;
                bout[i * 10] = d2;
            }

            var r = new Dictionary<int, B?>();
            for (i = 0; i < 10; ++i)
            {
                string s = "D1." + (i * 20).ToString();
                var d1 = new D1();
                d1.Sb = s;
                d1.Pb = i == 0 ? null : r[(i - 1) * 20];
                d1.Sd1 = s;
                d1.Pd1 = d1;
                r[i * 20] = d1;
            }
            return new ValueTask<(IReadOnlyDictionary<int, B?>, IReadOnlyDictionary<int, B?>)>((r, bout));
        }

        public ValueTask<PBase?> ExchangePBaseAsync(PBase? pb, Current current) => MakeValueTask(pb);

        public ValueTask<Preserved?> PBSUnknownAsPreservedAsync(Current current) =>
            new ValueTask<Preserved?>(new PSUnknown(5, "preserved", "unknown", null, new MyClass(15)));

        public ValueTask CheckPBSUnknownAsync(Preserved? p, Current current)
        {
            var pu = p as PSUnknown;
            TestHelper.Assert(pu != null);
            TestHelper.Assert(pu.Pi == 5);
            TestHelper.Assert(pu.Ps.Equals("preserved"));
            TestHelper.Assert(pu.Psu.Equals("unknown"));
            TestHelper.Assert(pu.Graph == null);
            TestHelper.Assert(pu.Cl != null && pu.Cl.I == 15);
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<Preserved?> PBSUnknownAsPreservedWithGraphAsync(Current current)
        {
            var graph = new PNode();
            graph.Next = new PNode();
            graph.Next.Next = new PNode();
            graph.Next.Next.Next = graph;

            var r = new PSUnknown(5, "preserved", "unknown", graph, null);
            return new ValueTask<Preserved?>(r);
        }

        public ValueTask CheckPBSUnknownWithGraphAsync(Preserved? p, Current current)
        {
            TestHelper.Assert(p is PSUnknown);
            var pu = (PSUnknown)p;
            TestHelper.Assert(pu.Pi == 5);
            TestHelper.Assert(pu.Ps.Equals("preserved"));
            TestHelper.Assert(pu.Psu.Equals("unknown"));
            TestHelper.Assert(pu.Graph != pu.Graph!.Next);
            TestHelper.Assert(pu.Graph.Next != pu.Graph!.Next!.Next);
            TestHelper.Assert(pu.Graph!.Next!.Next!.Next == pu.Graph);
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<Preserved?> PBSUnknown2AsPreservedWithGraphAsync(Current current)
        {
            var r = new PSUnknown2(5, "preserved", null);
            r.Pb = r;
            return new ValueTask<Preserved?>(r);
        }

        public ValueTask CheckPBSUnknown2WithGraphAsync(Preserved? p, Current current)
        {
            TestHelper.Assert(p is PSUnknown2);
            var pu = (PSUnknown2)p;
            TestHelper.Assert(pu.Pi == 5);
            TestHelper.Assert(pu.Ps.Equals("preserved"));
            TestHelper.Assert(pu.Pb == pu);
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<PNode?> ExchangePNodeAsync(PNode? pn, Current current) => MakeValueTask(pn);

        public ValueTask ThrowBaseAsBaseAsync(Current current)
        {
            var b = new B("sb", null);
            b.Pb = b;
            throw new BaseException("sbe", b);
        }

        public ValueTask ThrowDerivedAsBaseAsync(Current current)
        {
            var b = new B("sb1", null);
            b.Pb = b;

            var d = new D1("sb2", null, "sd2", null);
            d.Pb = d;
            d.Pd1 = d;

            throw new DerivedException("sbe", b, "sde1", d);
        }

        public ValueTask ThrowDerivedAsDerivedAsync(Current current)
        {
            var b = new B("sb1", null);
            b.Pb = b;

            var d = new D1("sb2", null, "sd2", null);
            d.Pb = d;
            d.Pd1 = d;

            throw new DerivedException("sbe", b, "sde1", d);
        }

        public ValueTask ThrowUnknownDerivedAsBaseAsync(Current current)
        {
            var d2 = new D2();
            d2.Sb = "sb d2";
            d2.Pb = d2;
            d2.Sd2 = "sd2 d2";
            d2.Pd2 = d2;

            throw new UnknownDerivedException("sbe", d2, "sude", d2);
        }

        public ValueTask ThrowPreservedExceptionAsync(Current current)
        {
            var ue = new PSUnknownException();
            ue.P = new PSUnknown2(5, "preserved", null);
            ue.P.Pb = ue.P;
            throw ue;
        }

        public ValueTask<Forward?> UseForwardAsync(Current current)
        {
            var f = new Forward();
            f.H = new Hidden();
            f.H.F = f;
            return new ValueTask<Forward?>(f);
        }

        // Type-inference helper method
        private static ValueTask<T> MakeValueTask<T>(T result) => new ValueTask<T>(result);
    }
}
