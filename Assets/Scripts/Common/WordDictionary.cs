using UnityEngine;
using System;

public interface IWordDictionary
{
    bool Search (string word, bool patternOnly = false);
}

public interface IWordDictionaryDS
{
    void Insert(string word);
    bool Search(string word, bool patternOnly = false);
}

public class WordDictionary : IWordDictionary
{
    private IWordDictionaryDS wordDictionary;

    private const string cDICTIONARY_FILE = "wordLists/wordList";
    private static readonly char[] cTRIM_CHARS = { '\r', '%' };


    public WordDictionary()
    {
        wordDictionary = new TrieNode ('\0');
        loadDictionary (cDICTIONARY_FILE);
    }

    public bool Search(string word, bool patternOnly = false)
    {
        return wordDictionary.Search (word, patternOnly);
    }

    #region Helper methods

    private void loadDictionary(String dictionaryFile)
    {
        Debug.Assert (dictionaryFile.Length != 0, "Dictionary cannot be loaded from empty filename");

        // load dictionary txt file
        TextAsset dictionaryAsset = Utility.LoadResource<TextAsset>(dictionaryFile);

        string[] words = dictionaryAsset.text.Split ('\n');

//        Debug.Log ("Loading words from " + dictionaryFile + " into dictionary");

        //load words in dictionary
        for (int i = 0; i < words.Length; i++) {
            wordDictionary.Insert (words [i].TrimEnd(cTRIM_CHARS));
        }
    }

    #endregion
}

/*
 * Optimizations:
 * Memory:
 *  1. Do not allocate children for leaf node - Done
 *  2. Allocate using List<index, node> - Done
 *  3. use constant TrieNodes for last letter - doesnt work since it corrupts existing paths
 *  4. idea to decrease memory 
 *      - use TrieNode directly and not keyValue
 *      - replace all duplicate nodes with one node
 *       - post process all nodes to find duplicates
 */

;
