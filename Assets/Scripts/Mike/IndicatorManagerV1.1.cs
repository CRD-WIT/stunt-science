// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using GameConfig;

// public class IndicatorManagerV1_1 : MonoBehaviour
// {
//     TextColorMode colorMode;
//     public Vector2 spawnPoint;
//     [SerializeField] float distance, time, velocity, distanceTraveled, lengthOfTime, elapsedTime;
//     public float fontSize = 4;
//     LineRenderer distanceLine1, distanceLine2, timeLine1, timeLine2, startLine, endLine, endTime;
//     GameObject distanceIndicatorTxt, timeIndicatorTxt, velocityIndicatorTxt;
//     [SerializeField] bool revealDistance = true, revealTime = true, revealVelocity = true;
//     public GameObject[] arrows = new GameObject[4];
//     public float timeStartPnt, timeEndPnt;
//     char required;
//     bool answered, isAnswerCorrect;
//     Vector2 veloTxtPoint;
//     QuestionControllerVThree qc;

//     // Start is called before the first frame update
//     void Start()
//     {
//         qc = FindObjectOfType<QuestionControllerVThree>();
//         distanceLine1 = transform.Find("DistanceLine1").GetComponent<LineRenderer>();
//         distanceLine2 = transform.Find("DistanceLine2").GetComponent<LineRenderer>();
//         timeLine1 = transform.Find("TimeLine1").GetComponent<LineRenderer>();
//         timeLine2 = transform.Find("TimeLine2").GetComponent<LineRenderer>();
//         startLine = transform.Find("StartAnnotationLine").GetComponent<LineRenderer>();
//         endLine = transform.Find("EndAnnotationLine").GetComponent<LineRenderer>();
//         endTime = transform.Find("EndAnnotationTime").GetComponent<LineRenderer>();
//         distanceIndicatorTxt = transform.Find("Distance").gameObject;
//         timeIndicatorTxt = transform.Find("Time").gameObject;
//         velocityIndicatorTxt = transform.Find("Velocity").gameObject;
//     }
//     public void ShowTimeAnnotation(bool show)
//     {
//         timeLine1.gameObject.SetActive(show);
//         timeLine2.gameObject.SetActive(show);
//         timeIndicatorTxt.gameObject.SetActive(show);
//         endTime.gameObject.SetActive(show);
//         arrows[2].gameObject.SetActive(show);
//     }
//     public void ShowDistanceAnnotation(bool show)
//     {
//         distanceLine1.gameObject.SetActive(show);
//         distanceLine2.gameObject.SetActive(show);
//         distanceIndicatorTxt.gameObject.SetActive(show);
//         startLine.gameObject.SetActive(show);
//         endLine.gameObject.SetActive(show);
//         arrows[0].gameObject.SetActive(show);
//         arrows[1].gameObject.SetActive(show);
//     }
//     public void Show(bool s)
//     {
//         foreach (var item in arrows)
//         {
//             item.SetActive(s);
//         }
//         distanceLine1.gameObject.SetActive(s);
//         distanceLine2.gameObject.SetActive(s);
//         distanceIndicatorTxt.SetActive(s);

//         timeLine1.gameObject.SetActive(s);
//         timeLine2.gameObject.SetActive(s);
//         timeIndicatorTxt.SetActive(s);

//         velocityIndicatorTxt.SetActive(s);

//         startLine.gameObject.SetActive(s);
//         endLine.gameObject.SetActive(s);
//         endTime.gameObject.SetActive(s);
//     }
//     public void SetPlayerPosition(Vector2 point)
//     {
//         veloTxtPoint = point;
//     }
//     public void Variables(float d, float t, float v, char whatIsAsk, float? EndPntOfTime)
//     {
//         colorMode = TextColorMode.Given;
//         answered = false;
//         switch (whatIsAsk)
//         {
//             case 'v':
//                 revealVelocity = false;
//                 revealTime = true;
//                 revealDistance = true;
//                 break;
//             case 'd':
//                 revealVelocity = true;
//                 revealTime = true;
//                 revealDistance = false;
//                 break;
//             case 't':
//                 revealVelocity = true;
//                 revealTime = false;
//                 revealDistance = true;
//                 break;
//             case 'N':
//                 revealVelocity = true;
//                 revealTime = true;
//                 revealDistance = true;
//                 break;
//             default:
//                 Debug.Log("<color=yellow>Variables</color>(distance value, time value, velocity value, symbol{d, v, t} if all value is revealed put 'N')");
//                 break;
//         }
//         required = whatIsAsk;
//         distance = d;
//         if (EndPntOfTime != null)
//             timeEndPnt = (float)EndPntOfTime;
//         else
//             timeEndPnt = distance;
//         time = t;
//         velocity = v;
//     }
//     public void AnswerIs(bool isCorrect)
//     {
//         if (isCorrect)
//         {
//             colorMode = TextColorMode.Correct;
//         }
//         else
//         {
//             colorMode = TextColorMode.Wrong;
//         }
//     }
//     public void IsRunning(float distanceCovered, float runTime, float? timeAnnotaionEndPoint)
//     {
//         answered = true;
//         distanceTraveled = distanceCovered;
//         elapsedTime = runTime;
//         if (timeAnnotaionEndPoint != null)
//         {
//             lengthOfTime = (float)timeAnnotaionEndPoint;
//         }
//         else
//         {
//             lengthOfTime = distanceCovered;
//         }
//     }
//     public void TimeHasDifferentPoints(bool yes)
//     {

//     }
//     public void PlayerAnswerIs(float answer)
//     {
//         // answered = false;
//         switch (required)
//         {
//             case 'v':
//                 velocity = answer;
//                 revealVelocity = true;
//                 break;
//             case 'd':
//                 distance = answer;
//                 revealDistance = true;
//                 break;
//             case 't':
//                 time = answer;
//                 revealTime = true;
//                 break;
//             default:
//                 Debug.Log("AnswerIs(answer)");
//                 break;
//         }
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         float dimensionTextLength = distanceIndicatorTxt.GetComponent<TextMeshPro>().text.Length;
//         float timeTextLength = timeIndicatorTxt.GetComponent<TextMeshPro>().text.Length;
//         if (!answered)
//         {
//             arrows[0].transform.position = new Vector2((distance + spawnPoint.x), spawnPoint.y);
//             arrows[1].transform.position = new Vector2(spawnPoint.x, spawnPoint.y);
//             distanceLine1.SetPosition(0, new Vector2(((distance / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
//             distanceLine1.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y));
//             distanceLine2.SetPosition(0, new Vector2(((distance / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
//             distanceLine2.SetPosition(1, new Vector2((distance + spawnPoint.x), spawnPoint.y));
//             startLine.SetPosition(0, new Vector2(spawnPoint.x, spawnPoint.y - 1));
//             startLine.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y + 0.5f));
//             endLine.SetPosition(0, new Vector2(spawnPoint.x + distance, spawnPoint.y - 1));
//             endLine.SetPosition(1, new Vector2(spawnPoint.x + distance, spawnPoint.y + 0.5f));
//             distanceIndicatorTxt.transform.position = new Vector2((distance / 2) + spawnPoint.x, spawnPoint.y);

//             arrows[2].transform.position = new Vector2(timeEndPnt + timeStartPnt, arrows[0].transform.position.y + 1);
//             timeLine1.SetPosition(0, new Vector2(((timeEndPnt / 2) - (0.2f * timeTextLength)) + timeStartPnt, spawnPoint.y + 1));//timeEndPnt = distance
//             timeLine1.SetPosition(1, new Vector2(timeStartPnt, spawnPoint.y + 1));
//             timeLine2.SetPosition(0, new Vector2(((timeEndPnt / 2) + (0.2f * timeTextLength)) + timeStartPnt, spawnPoint.y + 1));
//             timeLine2.SetPosition(1, new Vector2((timeEndPnt + timeStartPnt), spawnPoint.y + 1));
//             endTime.SetPosition(0, new Vector2(timeStartPnt + timeEndPnt, spawnPoint.y + .75f));
//             endTime.SetPosition(1, new Vector2(timeStartPnt + timeEndPnt, spawnPoint.y + 1.25f));
//             timeIndicatorTxt.transform.position = new Vector2((timeEndPnt / 2) + timeStartPnt, spawnPoint.y + 1);

//             arrows[3].transform.position = new Vector2(veloTxtPoint.x + 1.5f, veloTxtPoint.y + 3f);
//             velocityIndicatorTxt.transform.position = new Vector2(veloTxtPoint.x, veloTxtPoint.y + 3);
//             if (!revealDistance)
//                 distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.distance)}");
//             else
//                 distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distance, 2)}{qc.Unit(UnitOf.distance)}");
//             if (!revealTime)
//                 timeIndicatorTxt.GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.time)}");
//             else
//                 timeIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(time, 2)}{qc.Unit(UnitOf.time)}");
//             if (!revealVelocity)
//                 velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"?{qc.Unit(UnitOf.velocity)}");
//             else
//                 velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
//         }
//         else
//         {
//             arrows[0].transform.position = new Vector2((distanceTraveled + spawnPoint.x), spawnPoint.y);
//             arrows[1].transform.position = new Vector2(spawnPoint.x, spawnPoint.y);
//             distanceLine1.SetPosition(0, new Vector2(((distanceTraveled / 2) - (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
//             distanceLine1.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y));
//             distanceLine2.SetPosition(0, new Vector2(((distanceTraveled / 2) + (0.2f * dimensionTextLength)) + spawnPoint.x, spawnPoint.y));
//             distanceLine2.SetPosition(1, new Vector2((distanceTraveled + spawnPoint.x), spawnPoint.y));
//             startLine.SetPosition(0, new Vector2(spawnPoint.x, spawnPoint.y - 1));
//             startLine.SetPosition(1, new Vector2(spawnPoint.x, spawnPoint.y - 0.5f));
//             endLine.SetPosition(0, new Vector2(spawnPoint.x + distanceTraveled, spawnPoint.y - 1));
//             endLine.SetPosition(1, new Vector2(spawnPoint.x + distanceTraveled, spawnPoint.y - 0.5f));
//             distanceIndicatorTxt.transform.position = new Vector2((distanceTraveled / 2) + spawnPoint.x, spawnPoint.y);

//             arrows[2].transform.position = new Vector2(lengthOfTime + timeStartPnt, spawnPoint.y);
//             timeLine1.SetPosition(0, new Vector2(((lengthOfTime / 2) - (0.2f * timeTextLength)) + timeStartPnt, spawnPoint.y));
//             timeLine1.SetPosition(1, new Vector2(timeStartPnt, spawnPoint.y));
//             timeLine2.SetPosition(0, new Vector2(((lengthOfTime / 2) + (0.2f * timeTextLength)) + timeStartPnt, spawnPoint.y));
//             timeLine2.SetPosition(1, new Vector2((lengthOfTime + timeStartPnt), spawnPoint.y));
//             endTime.SetPosition(0, new Vector2(timeStartPnt + lengthOfTime, spawnPoint.y + .75f));
//             endTime.SetPosition(1, new Vector2(timeStartPnt + lengthOfTime, spawnPoint.y + 1.25f));
//             timeIndicatorTxt.transform.position = new Vector2((lengthOfTime / 2) + timeStartPnt, spawnPoint.y);

//             timeIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(elapsedTime, 2)}{qc.Unit(UnitOf.time)}");
//             velocityIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(velocity, 2)}{qc.Unit(UnitOf.velocity)}");
//             distanceIndicatorTxt.GetComponent<TextMeshPro>().SetText($"{System.Math.Round(distanceTraveled, 2)}{qc.Unit(UnitOf.distance)}");
//         }
//         distanceIndicatorTxt.GetComponent<TextMeshPro>().fontSize = fontSize;
//         velocityIndicatorTxt.GetComponent<TextMeshPro>().fontSize = fontSize;
//         timeIndicatorTxt.GetComponent<TextMeshPro>().fontSize = fontSize;

//         arrows[3].transform.position = new Vector2(veloTxtPoint.x + 1.5f, veloTxtPoint.y + 3f);
//         velocityIndicatorTxt.transform.position = new Vector2(veloTxtPoint.x, veloTxtPoint.y + 3);

//         qc.SetColor(timeIndicatorTxt.GetComponent<TMP_Text>(), colorMode);
//         qc.SetColor(distanceIndicatorTxt.GetComponent<TMP_Text>(), colorMode);
//         qc.SetColor(velocityIndicatorTxt.GetComponent<TMP_Text>(), colorMode);
//         distanceLine1.startColor = qc.getHexColor(colorMode);
//         distanceLine1.endColor = qc.getHexColor(colorMode);
//         distanceLine2.startColor = qc.getHexColor(colorMode);
//         distanceLine2.endColor = qc.getHexColor(colorMode);
//         timeLine1.startColor = qc.getHexColor(colorMode);
//         timeLine1.endColor = qc.getHexColor(colorMode);
//         timeLine2.startColor = qc.getHexColor(colorMode);
//         timeLine2.endColor = qc.getHexColor(colorMode);
//         endTime.startColor = qc.getHexColor(colorMode);
//         endTime.endColor = qc.getHexColor(colorMode);
//         startLine.startColor = qc.getHexColor(colorMode);
//         startLine.endColor = qc.getHexColor(colorMode);
//         endLine.startColor = qc.getHexColor(colorMode);
//         endLine.endColor = qc.getHexColor(colorMode);
//         foreach (var item in arrows)
//         {
//             item.GetComponent<SpriteRenderer>().color = qc.getHexColor(colorMode);
//         }
//     }
// }
