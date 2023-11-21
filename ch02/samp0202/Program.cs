using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

// HTTPクライアントの生成
var client = new HttpClient();
// OpenAI APIキーの設定
string apiKey = 
"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";
// リクエストヘッダーにAPIキーを追加
client.DefaultRequestHeaders.Add(
"Authorization", $"Bearer {apiKey}");
// 要約する文章
string textToSummarize = """
OpenAIのGPT（Generative Pretrained Transformer）は、自然言語処理（NLP）の分野で開発された先進的な機械学習モデルです。以下に、GPTについての主要な特徴と概要を解説します。

GPTの基本概念
トランスフォーマーモデル: GPTは「トランスフォーマー」と呼ばれるアーキテクチャに基づいています。このアーキテクチャは、特に大量のテキストデータを効率的に処理する能力に優れています。

事前学習とファインチューニング: GPTは、大規模なテキストコーパスを使用して事前に学習（プリトレーニング）されます。その後、特定のタスクやデータセットに対してファインチューニングを行うことで、さまざまな言語タスクに対応します。

自己回帰モデル: GPTは自己回帰モデルであり、過去の入力（前の単語）に基づいて次の単語を予測します。この特性により、テキスト生成に特に適しています。

GPTの進化
GPT-1: OpenAIによる初めてのGPTモデル。自然言語理解と生成のための基礎となりました。

GPT-2: より大きなデータセットとモデルサイズを持ち、テキスト生成の品質が大幅に向上しました。GPT-2はその生成したテキストのリアリズムと連続性で注目を集めました。

GPT-3: さらに大規模なモデルで、数十億のパラメータを持ちます。このモデルは、非常に多様な言語タスクを高い精度でこなすことができ、テキスト生成の分野で革新をもたらしました。

GPT-4以降: さらに進化したモデルは、より大きなデータセットと洗練された学習アルゴリズムを用いて、性能の向上を目指しています。

応用分野
テキスト生成: 物語、詩、記事など、さまざまなスタイルのテキストを生成します。
言語翻訳: 高品質な翻訳タスクに対応します。
質問応答: 人間のように自然なやり取りで質問に答えます。
テキスト要約: 長い文章を短く要約します。
利点と課題
利点: 多様な言語タスクに柔軟に対応し、高い精度で自然なテキストを生成できます。
課題: トレーニングに非常に大きなデータセットと計算資源を必要とし、場合によってはバイアスや不正確な情報を生成する可能性があります。
GPTは、AIと自然言語処理の分野における重要な進歩を象徴しており、今後もその応用範囲は広がり続けることが予想されます。
""";
// GPTを使用して要約を生成するためのプロンプト
string prompt = $"次の文章を要約してください:\n{textToSummarize}\n要約:";

// リクエストボディの作成
var requestBody = new
{
    model = "gpt-3.5-turbo-instruct",
    prompt = prompt,
    max_tokens = 500, // 出力する文章の長さを調節
    temperature = 0.7
};

// リクエストをJSON形式に変換
string json = JsonSerializer.Serialize(requestBody);
var content = new StringContent(json, Encoding.UTF8, "application/json");
// OpenAI APIにリクエストを送信
HttpResponseMessage response = await client.PostAsync(
"https://api.openai.com/v1/completions", content);

// 応答を取得
string result = await response.Content.ReadAsStringAsync();
// 応答を表示(JSON形式)
Console.WriteLine(result);

