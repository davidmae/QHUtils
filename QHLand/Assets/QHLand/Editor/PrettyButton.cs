using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QHLand
{
    public class PrettyButton
    {
        public string name;
        public bool keepColor;

        private int ticks;
        private float time;
        private int tticks;
        public bool executeTicks;

        public PrettyButton(string name, int ticks)
        {
            this.name = name;
            this.ticks = ticks;
            executeTicks = true;
        }

        public bool PButton(Color a, Color b, float t)
        {
            if (executeTicks)
            {
                time += t;
                if (time > 3) time = 0;

                if ((ticks == -1 || tticks < ticks))
                {
                    if (time <= 1)
                        GUI.color = Color.Lerp(a, b, time);
                    else if (time > 1 && time <= 2)
                        GUI.color = Color.Lerp(b, a, time - 1);
                    else
                    {
                        time = 0;
                        tticks += 1;
                    }
                }
            }
            else
                GUI.color = b;

            if (GUILayout.Button(name))
            {
                time = 0f;
                tticks = 0;

                GUI.color = a;
                return true;
            }

            GUI.color = a;
            return false;
        }

        public bool PButton(bool condition, Color a, Color b, float t)
        {
            if (executeTicks)
            {
                time += t;
                if (time > 3) time = 0;

                if (condition && (ticks == -1 || tticks < ticks))
                {
                    if (time <= 1)
                        GUI.color = Color.Lerp(a, b, time);
                    else if (time > 1 && time <= 2)
                        GUI.color = Color.Lerp(b, a, time - 1);
                    else
                    {
                        time = 0;
                        tticks += 1;
                    }
                }
            }
            else
                GUI.color = b;

            if (GUILayout.Button(name))
            {
                time = 0f;
                tticks = 0;

                GUI.color = a;
                return true;
            }

            GUI.color = a;
            return false;
        }

        public bool PButton(bool condition, Color a, Color b, float t, params GUILayoutOption[] options)
        {
            if (executeTicks)
            {
                time += t;
                if (time > 3) time = 0;

                if (condition && (ticks == -1 || tticks < ticks))
                {
                    if (time <= 1)
                        GUI.color = Color.Lerp(a, b, time);
                    else if (time > 1 && time <= 2)
                        GUI.color = Color.Lerp(b, a, time - 1);
                    else
                    {
                        time = 0;
                        tticks += 1;
                    }
                }
            }
            else
                GUI.color = b;

            if (GUILayout.Button(name, options))
            {
                time = 0f;
                tticks = 0;

                GUI.color = a;

                return true;
            }

            GUI.color = a;
            return false;
        }

        public void Repaint()
        {
            time = 0f;
            tticks = 0;
        }

        public void Repaint(int ticks)
        {
            Repaint();
            this.ticks = ticks;
        }

        public void ShowFade(Color a, Color b, float t)
        {
            if (keepColor)
                return;

            time += t;
            if (time > 3) time = 0;

            if ((ticks == -1 || tticks < ticks))
            {
                if (time <= 1)
                    GUI.color = Color.Lerp(a, b, time);
                else if (time > 1 && time <= 2)
                    GUI.color = Color.Lerp(b, a, time - 1);
                else
                {
                    time = 0;
                    tticks += 1;
                }
            }
        }


    }
}