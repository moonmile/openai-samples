using SharpToken;

string text = "この文章のトークン数はいくつですか？";
// string text = "How many tokens are in this sentence?";
var encoding = GptEncoding.GetEncodingForModel("gpt-4");
List<int> tokens = encoding.Encode(text);
Console.WriteLine(tokens.Count);
