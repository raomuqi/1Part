using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CommonUtils
{

    #region RichText display for debugging
    static StringBuilder outText = new StringBuilder();
    const string prefix = "<b><color=yellow>------->{<color=orange> ";
    const string postfix = " </color>}<------</color></b>";
    /// <summary>
    /// For debugging
    /// </summary>
    public static string normalText
    {
        get { return string.Format("{0}JACK ASSIGN{1}", prefix, postfix); }
    }
    /// <summary>
    /// For debugging
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string ShowText(params object[] args)
    {
        outText.Length = 0;
        outText.Append(prefix);
        for (int i = 0; i < args.Length; i++)
        {
            outText.Append(args[i]);
        }
        outText.Append(postfix);
        return outText.ToString();
    }
    #endregion
}
