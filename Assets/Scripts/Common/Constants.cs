using UnityEngine;
using System.Collections;

public static class Constants
{
    public const int NUM_ROWS = 15;
    public const int NUM_COLUMNS = 5;

    public const int START_ROW_NUM = 1;
    public const int START_COLUMN_NUM = 0;
    public static readonly Vector3 InvalidVector = Vector3.one * -1f;

    public const float BLOCK_Z = 18.0f;
    public const float POWERUP_Z = BLOCK_Z - 1f;
    public const int BLOCK_LAYERMASK = 1 << 8;
    public const float ZERO_THRESHOLD = 0.25f;
    public const string DICTIONARY_FILE = "wordLists/wordList";
    public static readonly char[] TRIM_CHARS = { '\r', '%' };
    public static int MIN_WORD_LENGTH = 2;
    public static double SILVER_WATCH_TIME_DURATION = 14000;
    public static double GOLD_WATCH_TIME_DURATION = 25000;
    public const float WATCH_SLOW_SPEED = 1f;
    public const float REMOVE_LAG_COROUTINE = 0.1f; // number of seconds between each coroutine yield
    public const float FADING_TEXT_DURATION = 2.5f; // 2f; // 

    public const int MAX_LETTER_MULTIPLIER = 5;
    public const int MAX_WORD_MULTIPLIER = 5;

    public static class Powerups
    {
        public const int JOKER_EXTENTS = 1;
        public const int WILDJOKER_EXTENTS = 2;
        public const int JOKER_RINSETIMES = 5; // number of times cards will be shuffled
    }

    public static class WordLengthBonus
    {
        public const int LENGTH_7_BONUS = 50;
        public const int LENGTH_6_BONUS = 25;
        public const int LENGTH_5_BONUS = 10;
    }

    public static class Scenes
    {
        public const string TitleScene = "TitleScene";
        public const string LevelSelectScene = "LevelSelectScene";
        public const string GameplayScene = "GameplayScene";
        public const string BoardOverScene = "BoardOverScene";
        public const string PauseScene = "PauseScene";
        public const string SettingsScene = "SettingsScene";
        public const string CreditsScene = "CreditsScene";
        public const string TutorialScene = "TutorialScene";
        public const string ComingSoonScene = "ComingSoonScene";
        public const string UpsellScene = "UpsellScene";
        public const string AdScene = "AdScene";
    }

    public static class Persist
    {
        public const string VERSION = "VERSION";
        public const string PREVIOUS_RUN = "PREVIOUS_RUN";
        public const string CURRENT_BOARD_ID = "CURRENT_BOARD_ID";
        public const string FX_VOLUME = "FX_VOLUME";
        public const string MUSIC_VOLUME = "MUSIC_VOLUME";
        public static class Board
        {
            public const string HIGH_SCORE = "_HIGH_SCORE";
            public const string STARS = "_STARS";
            public const string STATUS = "_STATUS";
        }
        public static string PAID_PATH = Application.persistentDataPath + "/sikka.dt";
        public static string KHAREEDAHUADAAM = "KHAREEDAHUADAAM";

        public static string MOONS_PATH = Application.persistentDataPath + "/Moontime.dt";
    }

    public static class Payment
    {
        /// <summary>
        /// The tier1 board threshold (including this id)
        /// </summary>
        public const int Tier1BoardThreshold = 20;

        /// <summary>
        /// The tier2 board threshold (including this id)
        /// </summary>
        public const int Tier2BoardThreshold = 50;

        /// <summary>
        /// The tier3 board threshold (including this id)
        /// </summary>
        public const int Tier3BoardThreshold = 80;

        public const string Tier1ProductId = "adFree";
        public const string Tier15ProductId = "moreLevelsStep";
        public const string Tier2ProductId = "moreLevels";

        // TODO: These three need to be updated
        public const string Tier3ProductId = "tier3";
        public const string Tier1To3ProductId = "tier1To3";
        public const string Tier25ProductId = "tier25";
    }
    public const int PurchasedUpsellFrequency = 6;

    public static class BlockStateColors
    {
        public static readonly Color32 NotSelected = new Color32(0xD1, 0x7a, 0x30, 0xFF);
        public static readonly Color32[] NotSelecteds = {
            NotSelected, NotSelected, NotSelected, NotSelected
        };

        private static readonly Color32 Selected = new Color32(0xD2, 0xA1, 0x30, 0xFF);
        public static readonly Color32[] Selecteds = {
            Selected, Selected, Selected, Selected
        };

        private static readonly Color32 JokerGray = new Color32(166, 184, 132, 0xFF);
        public static readonly Color32[] JokerGrays = {
            JokerGray, JokerGray, JokerGray, JokerGray
        };

        private static readonly Color32 JokerGreen = new Color32(0, 174, 174, 0xFF);
        public static readonly Color32[] JokerGreens = {
            JokerGreen, JokerGreen, JokerGreen, JokerGreen
        };

        public static readonly Color32 Gray = new Color32(145, 145, 145, 0xFF);
        public static readonly Color32[] Grays = {
            Gray, Gray, Gray, Gray
        };

        /// <summary>
        /// The bright multipliers indexed from 0 - 5
        /// </summary>
        public static readonly Color[] BrightMultipliers = new Color[] {
            new Color(),
            new Color(),
            new Color (0.09f, 0.71f, 0.96f, 1), // blue
			new Color (0.85f, 0.678f, 0.423f, 1), // brown
			new Color (0.443f, 0.898f, 0.776f, 1), // green
			new Color (0.941f, 0.458f, 0.458f, 1), // red
		};

        private static readonly Color32 Multiplier0x = new Color32(0x3A, 0x3A, 0X3A, 0XFF);
        private static readonly Color32 Multiplier2x = new Color32(0x35, 0x66, 0x8A, 0xFF);
        private static readonly Color32 Multiplier3x = new Color32(0X8A, 0X68, 0X35, 0XFF);
        private static readonly Color32 Multiplier4x = new Color32(0X35, 0X8A, 0X74, 0XFF);
        private static readonly Color32 Multiplier5x = new Color32(0X8A, 0X35, 0X35, 0XFF);

        public static readonly Color32[][] Multiplier = new Color32[][]{
            new Color32[]{
                Multiplier0x,Multiplier0x,Multiplier0x,Multiplier0x
            }, // 0x

			new Color32[]{}, // 1x

			new Color32[]{
                Multiplier2x, Multiplier2x, Multiplier2x, Multiplier2x
            }, // 2x - bluish

			new Color32[]{
                Multiplier3x, Multiplier3x, Multiplier3x, Multiplier3x
            }, // 3x - brownish

			new Color32[]{
                Multiplier4x, Multiplier4x, Multiplier4x, Multiplier4x
            }, // 4x - greenish

			new Color32[]{
                Multiplier5x, Multiplier5x, Multiplier5x, Multiplier5x
            }, // 5x - redish
		};
    }
}
