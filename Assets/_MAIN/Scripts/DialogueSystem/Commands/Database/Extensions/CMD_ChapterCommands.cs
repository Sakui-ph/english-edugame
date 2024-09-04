using COMMANDS;
using DIALOGUE;
using System;
public class CMD_ChapterCommands : CMD_DatabaseExtension
{   
    private static DialogueSystem ds => VisualNovelSL.services.dialogueSystem;

    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("queuechapter", new Action<string>(QueueChapter));
        database.AddCommand("queuedirectory", new Action<string>(QueueDirectory));
    }

    public static void QueueChapter(string chapterName)
    {
        ds.chapterManager.QueueFile(chapterName);
    }

    public static void QueueDirectory(string directory)
    {
        ds.chapterManager.QueueDirectory(directory);
    }
}


