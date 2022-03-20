using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calcultator : ButtonListener
{
    string Expression { set => GetComponentInChildren<TextMesh>().text = value; get => GetComponentInChildren<TextMesh>().text;}
    private void Start()
    {
        Expression = "";
    }
    public override void OnPressed(string arg)
    {
        if (arg == "C") { Expression = "";return; }
        else if (arg == "=")
        {
            try
            {
                string[] tokens = Expression.Split(' ');
                int value = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i] == "+")
                        value += int.Parse(tokens[i + 1]);
                    else if (tokens[i] == "-")
                        value -= int.Parse(tokens[i + 1]);
                    else if (i == 0)
                        value = int.Parse(tokens[i]);
                }
                Expression = value.ToString();
            }
            catch
            {
                Expression = "err";
            }
            return;
        }
        Expression += arg;
    }
}
