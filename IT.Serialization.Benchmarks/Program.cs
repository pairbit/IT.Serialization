using IT.Serialization.Benchmarks;

var bench = new SerializeBenchmark();

#if NETCOREAPP3_1_OR_GREATER

//var mp = bench.MemoryPack_Deserialize();
//var hp = bench.HyperSerializer_Deserialize();
//var bp = bench.BinaryPack_Deserialize();

//if (!hp!.Equals(mp)) throw new InvalidOperationException();
//if (!bp!.Equals(mp)) throw new InvalidOperationException();

#endif

var jp = bench.Json_Deserialize()!;

if (!jp.Equals(bench.MessagePack_Deserialize())) throw new InvalidOperationException();
if (!jp.Equals(bench.Utf8Json_Deserialize())) throw new InvalidOperationException();

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Serialization.Benchmarks.SerializeBenchmark));