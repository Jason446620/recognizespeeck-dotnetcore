using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace vscodeConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await RecognizeSpeechAsync();
        }
        static async Task RecognizeSpeechAsync()
        {
            // Configure the subscription information for the service to access.
            // Use either key1 or key2 from the Speech Service resource you have created
            var config = SpeechConfig.FromSubscription("subscriptionkey", "region");

            // Setup the audio configuration, in this case, using a file that is in local storage.
            try
            {
                using (var audioInput = AudioConfig.FromWavFileInput("./narration.wav"))
                // Pass the required parameters to the Speech Service which includes the configuration information
                // and the audio file name that you will use as input
                using (var recognizer = new SpeechRecognizer(config, audioInput))
                {
                    Console.WriteLine("Recognizing first result...");
                    var result = await recognizer.RecognizeOnceAsync();

                    switch (result.Reason)
                    {
                        case ResultReason.RecognizedSpeech:
                            // The file contained speech that was recognized and the transcription will be output
                            // to the terminal window
                            Console.WriteLine($"We recognized: {result.Text}");
                            break;
                        case ResultReason.NoMatch:
                            // No recognizable speech found in the audio file that was supplied.
                            // Out an informative message
                            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                            break;
                        case ResultReason.Canceled:
                            // Operation was cancelled
                            // Output the reason
                            var cancellation = CancellationDetails.FromResult(result);
                            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                            if (cancellation.Reason == CancellationReason.Error)
                            {
                                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                                Console.WriteLine($"CANCELED: Did you update the subscription info?");
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
            
        }
    }
}
