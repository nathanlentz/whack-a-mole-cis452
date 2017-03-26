using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace Wintergreen.Sounds
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> _soundLibrary;

        /// <summary>
        /// Used to load the .xnb sound files from the Content Pipeline into the soundLibrary
        /// </summary>
        /// <param name="game"></param>filename of the sound file to be loaded
        /// <returns>0 if sounds successfully loaded
        /// -100 if no files located in Content\sounds</returns>
        public static int LoadSounds(Game game)
        {
            var temp = Directory.CreateDirectory(game.Content.RootDirectory);
            string[] soundNamePaths = Directory.GetFiles(game.Content.RootDirectory + "\\sounds");
            _soundLibrary = new Dictionary<string, SoundEffect>();

            if(soundNamePaths.Length == 0)
            { 
                //TODO: Add logging code here
                Console.WriteLine("No files found");
                return -100;
            }
            foreach (string soundNamePath in soundNamePaths)
            {
                string soundName = "";
                if (soundNamePath.EndsWith(".xnb"))
                {
                    soundName = Path.GetFileNameWithoutExtension(soundNamePath);
                    _soundLibrary.Add(soundName, game.Content.Load<SoundEffect>("sounds\\" + soundName));
                }
                else
                {
                    //TODO: Add logging code here
                    Console.WriteLine("Error: {0} has invalid file type. Must be of type .wav. Skipping..." , Path.GetFileName(soundNamePath));       
                }
            }
            return 0;
            
        }

        /// <summary>
        /// Creates a SoundEffectInstance based on a SoundEffect Object and plays the sound associated with it
        /// </summary>
        /// <param name="sound"></param>Name of SoundEffect in dictionary to be played
        /// <param name="isLooped"></param>Attribute to the instance which tells if the sound should be looped
        /// <param name="volume"></param>Attribute to the instance which tells the volume of the sound
        /// <returns>The SoundEffectInstance of the sound which is playing</returns>
        public static SoundEffectInstance PlaySound(string soundName, bool isLooped, float volume = 1f)
        {
            SoundEffect soundEffect;

            if(_soundLibrary.TryGetValue(soundName, out soundEffect))
            {
                SoundEffectInstance soundInstance = soundEffect.CreateInstance();
                soundInstance.IsLooped = isLooped;
                soundInstance.Volume = volume;
                soundInstance.Play();
                return soundInstance;
            }
            else
            {
                //TODO: Add logging code here
                throw new ArgumentException("Error: sound file not found in dictionary", soundName);
            }            
        }

        /// <summary>
        /// Creates a SoundEffectInstance based on a SoundEffect Object and plays the sound associated with it
        /// </summary>
        /// <param name="sound"></param>Name of SoundEffect in dictionary to be played
        /// <param name="isLooped"></param>Attribute to the instance which tells if the sound should be looped
        /// <param name="pan"></param>Attribute to the instance which tells the pan of the sound
        /// <param name="pitch"></param>Attribute to the instance which tells the pitch of the sound
        /// <param name="volume"></param>Attribute to the instance which tells the volume of the sound
        /// <returns>The SoundEffectInstance of the sound which is playing</returns>
        public static SoundEffectInstance PlaySound(string soundName, bool isLooped, float pan, float pitch, float volume = 1f)
        {
            SoundEffect soundEffect;
            if (_soundLibrary.TryGetValue(soundName, out soundEffect))
            {
                SoundEffectInstance soundInstance = soundEffect.CreateInstance();
                soundInstance.IsLooped = isLooped;
                soundInstance.Pan = pan;
                soundInstance.Pitch = pitch;
                soundInstance.Volume = volume;
                soundInstance.Play();
                return soundInstance;
            }
            else
            {   
                //TODO: Add logging code here
                throw new ArgumentException("Error: sound file not found in dictionary", "soundName");
            }
        }

        /// <summary>
        /// Returns a SoundEffectInstance of a SoundEffect
        /// </summary>
        /// <param name="soundName">Name of the sound effect to be returned.</param>
        /// <returns>SoundEffectInstance of the SoundEffect or null if no SoundEffect was found in the library</returns>
        public static SoundEffectInstance CreateInstance(string soundName)
        {
            SoundEffect soundEffect;
            if (_soundLibrary.TryGetValue(soundName, out soundEffect))
            {
                return soundEffect.CreateInstance();
            }

            return null;

        }

        /// <summary>
        /// Fades the track in or out, depending on the state of the game
        /// </summary>
        /// <param name="fadeIn"></param>bool which tells whether or not the sound should fade in or out
        /// <param name="sound"></param>SoundEffectInstance which should fade in or out
        /// <param name="fadeSpeed"></param>tells the rate at which the sound should fade in or out (defaults to 0.004f)
        public static void FadeSound(bool fadeIn, SoundEffectInstance sound, float fadeSpeed = 0.004f)
        {
            if (fadeIn) 
            {
                if (sound.Volume < 0.99f)
                {
                    sound.Volume += fadeSpeed;
                }
            }
            else 
            {
                if (sound.Volume > 0.01f)
                {
                    sound.Volume -= fadeSpeed;
                }
                else
                {
                    sound.Stop();
                }
            }
        }

    }
}
