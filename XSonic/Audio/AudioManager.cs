using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using System.Collections.Generic;

namespace XSonic.Audio
{
    /// <summary>
    /// 
    /// </summary>
	public class AudioManager : GameComponent
	{
		static ContentManager content;
		public static Song currentSong;
		public bool conentLoaded = false, playMusic = true, playFX = true;

		List<string> songList = new List<string> { "stillalive-1", "stillalive-2", "stillalive-3", "stillalive-4", "stillalive-6", "stillalive-7", "stillalive-8", "stillalive-9", "thatwasgreat", "sparta" };
		List<string> specialSongList = new List<string> { "gameover", "win" };//songs that shouldn't appear in the playlist
		Random r;
		public static Dictionary<string, SoundEffect> effects = new Dictionary<string, SoundEffect>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
		public AudioManager(Game game) : base(game)
		{
            game.Components.Add(this);
            game.Services.AddService(typeof(AudioManager), this);
			r = new Random();
			MediaPlayer.Volume = .25f;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
		public void PlaySpecialSong(string name)
		{
			try
			{
				MediaPlayer.Play(content.Load<Song>(@"sounds\songs\" + name));
			}
			catch (Exception) { }
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectname"></param>
		public void PlaySound(string effectname)
		{
			if (playFX)
			{
				SoundEffect effect;
				if (effects.TryGetValue(effectname, out effect))
					effect.Play(MediaPlayer.Volume - .10f);
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public void Play()
		{
			if (playMusic)
			{
				MediaPlayer.Play(currentSong);
			}
		}

		/// <summary>
		/// Play a song
		/// </summary>
		/// <param name="assetName">The name of the song asset</param>
		public void PlaySong(string assetName)
		{
			if (playMusic)
			{
				MediaPlayer.Stop();
				try
				{
					currentSong = content.Load<Song>(@"sounds\songs\" + assetName);
					MediaPlayer.Play(currentSong);
				}
				catch (Exception)
				{
					//unable to load the song...
				}
			}
		}
/// <summary>
/// 
/// </summary>
/// <param name="cm"></param>
		public void LoadContent(ContentManager cm)
		{
			if (!conentLoaded)
			{
				content = cm;

				effects.Add("1up", content.Load<SoundEffect>(@"sounds\effect\1up"));
				effects.Add("jump", content.Load<SoundEffect>(@"sounds\effect\jump"));
				effects.Add("coins", content.Load<SoundEffect>(@"sounds\effect\coins"));

				effects.Add("1down", content.Load<SoundEffect>(@"sounds\effect\1down"));
				conentLoaded = true;
			}

		}

		/// <summary>
		/// Play a random song that is listed in the songlist
		/// </summary>
		public void PlayRandomSong()
		{
			if (MediaPlayer.State != MediaState.Playing && playMusic)
			{

				int song = r.Next(0, songList.Count);
				PlaySong(songList[song]);
			}
			else if (!playMusic)
				Stop();
		}

		/// <summary>
		/// Stop the currently playing song
		/// </summary>
		public void Stop()
		{
			MediaPlayer.Stop();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
		public static void SetVolume(float volume)
		{
			MediaPlayer.Volume = volume;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gametime"></param>
		public override void Update(GameTime gametime)
		{
			KeyboardState keyboard = Keyboard.GetState();
			//toggle sound on and off
			if (!keyboard.IsKeyUp(Keys.P))
			{
				if (playMusic)
					playMusic = false;
				else
					playMusic = true;
			}
			//change the volume
			if (keyboard.IsKeyDown(Keys.OemMinus))
				MediaPlayer.Volume -= .01f;
			else if (keyboard.IsKeyDown(Keys.OemPlus))
				MediaPlayer.Volume += .01f;

            PlayRandomSong();
		}
	}
}
