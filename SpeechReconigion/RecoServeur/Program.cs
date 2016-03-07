using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using System.Speech.Recognition;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Microsoft.Speech.Recognition;
using System.Globalization;

namespace RecoServeur
{
	class Program
	{
		public static SpeechRecognitionEngine speechRecognitionEngine;
		public static string reception = "#";
		public static string endWord = "Terminer"; 
		public static string ipServer = "127.0.0.1";
		public static byte[] data = new byte[512];
		public static int port = 26000;
		public static double validity = 0.70f;

		public static void Main(string[] args)
		{

            //speechRecognitionEngine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            speechRecognitionEngine = new SpeechRecognitionEngine(new CultureInfo("en-US"));
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); //내가 추가

            try
			{
				// create the engine
				// hook to events
				//speechRecognitionEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
				speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_SpeechRecognized);

				// load dictionary
				try
				{
					Choices texts = new Choices();
					string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\grammar.txt");
					foreach (string line in lines)
					{
						//reco UDP Port
						if (line.StartsWith("#P"))
						{
							var parts = line.Split(new char[] { ' ' });
							port = Convert.ToInt32(parts[1]);
							Console.WriteLine("Port : " + parts[1]);
							continue;
						}
						// Reco endWord
						if (line.StartsWith("#E"))
						{
							var parts = line.Split(new char[] { ' ' });
							endWord = parts[1];
							Console.WriteLine("End Word : " + parts[1]);
							continue;
						}
						// Reco IP server
						if (line.StartsWith("#I"))
						{
							var parts = line.Split(new char[] { ' ' });
							ipServer = parts[1];
							Console.WriteLine("ipServer : " + parts[1]);
							continue;
						}
						// Reco validity
						if (line.StartsWith("#V"))
						{
							var parts = line.Split(new char[] { ' ' });
							validity = Convert.ToInt32(parts[1])/100.0f;
							Console.WriteLine("Validity : " + parts[1]);
							continue;
						}

						// skip commentblocks and empty lines..
						if (line.StartsWith("#") || line == String.Empty) continue;

						texts.Add(line);
					}
                    //Grammar wordsList = new Grammar(new GrammarBuilder(texts));
                    //wordsList.Culture = Thread.CurrentThread.CurrentCulture;
                    GrammarBuilder gb = new GrammarBuilder(texts);
                    gb.Culture = Thread.CurrentThread.CurrentCulture;
                    gb.Culture = new CultureInfo("en-US");

                    Grammar wordsList = new Grammar(gb);


                    speechRecognitionEngine.LoadGrammar(wordsList);
				}
				catch (Exception ex)
				{
					throw ex;
					System.Environment.Exit(0);
				}

				// use the system's default microphone
				speechRecognitionEngine.SetInputToDefaultAudioDevice();
				// start listening
				speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message, "MicroPhone?");
				speechRecognitionEngine.RecognizeAsyncStop();
				speechRecognitionEngine.Dispose();
				System.Environment.Exit(0);
			}
			// UDP init
			Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ipServer), port);
			// Ready
			Console.WriteLine("Ready.....");

			while (true)
			{
				if (reception != "#")
				{
					data = Encoding.ASCII.GetBytes(reception);
					server.SendTo(data, iep);
					//Console.WriteLine("Sending : "+reception);
					reception = "#";
				}
				Thread.Sleep(2);
			}

		} // main

		public static  void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
		{
			Console.WriteLine(e.AudioLevel);
		}


		public static void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			if (e.Result.Confidence >= validity)
			{
				reception = e.Result.Text;
				//Console.WriteLine(reception);
				// Arret du serveur si mot magique
				if (e.Result.Text == endWord)
				{
					speechRecognitionEngine.RecognizeAsyncStop();
					speechRecognitionEngine.Dispose();
					System.Environment.Exit(0);
				}
			}
			else
			{
				reception = "#";
			}
		}

	} // Program
} // RecoServeur
// End source

