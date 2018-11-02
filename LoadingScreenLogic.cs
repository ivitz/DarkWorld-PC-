using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenLogic : MonoBehaviour {

    public static string sceneName;
    public Slider slider;
    //public GameObject playButton;
    public Text loadingText;
    public Text hintText;
    public Text loreText;

    private bool pressMouseButt;

    [TextArea(2, 5)]
    public string[] hints;

    private AsyncOperation asyncLoad = null;



    private bool allowLoad;
    private int hintSelected;
	// Use this for initialization
	void Start () {

        hintSelected = Random.Range(0, hints.Length);
        hintText.text = hints[hintSelected];
     

        if (sceneName != null)
        {
            StartCoroutine(LoadYourAsyncScene(sceneName));
            //playButton.SetActive(false);
        } /*
        else
        {
            sceneName = "World01-2(Swamp)";
            StartCoroutine(LoadYourAsyncScene(sceneName));
            //playButton.SetActive(false);
        }
*/
        switch (sceneName)
        {
            case "World00":
                loreText.text = "\tYou see the darkness all around just as you stand there in the middle of nowhere. How much time passed, you cannot tell. Suddenly you woke up like from a dream, remembering nothing of the transition to consciousness. Your vision is blurred and your eyes hurt much. You can see something glowing with the dark light far away. You cannot tell what it is but it is calling for you… there are many voices in the air.";
                break;
            case "World01":
                loreText.text = "\tIt was a strange way to a huge portal. You went through. The darkness was all around you. Suddenly you hear a strange noise. It is growing, shaping into familiar sounds. Now you clearly can hear a thunder. Wind is howling. You see the dim light illuminating strange shapes. The vision starts to make sense and you find yourself in a tall grass with dark sky above. You arrive to the new world - unknown and ominous. Why are you here? How did you get here? Is this a dream? It does not seem so. You need answers…";
                break;
            case "World01-2(Swamp)":
                loreText.text = "\tYou traveled far with the guide beside you. Three days passed, you barely spoke, and the road was dark, until you reached a shore with the bright water. The guide sailed a boat and you swam for another two days. You could not sleep and asked about an Old Knight, but received no answer. You wonder will you meet him again. Finally, a strange land appeared before you. What lies ahead is unknown though it appears you have no other choice but to move on.";
                break;
            case "World02(Forest)":
                loreText.text = "\tThe path you took between the mountains was dim. The mist was rising all around as you walked deep into the forest. You tried to rest but you were troubled much and could not sleep. Thoughts of an Old Knight did not leave your head. Who is he? What is the light he speaks of? You were completely lost and wished the guide would be by your side. Where did he go? He was to follow you. Finally, you saw someone on the road…";
                break;
            case "World02-2(Beast)":
                loreText.text = "\tAs the Hunter died, the mist was almost gone and the night fell on the forest. You had to rest and stayed until the morning. You barely slept. You had no dreams but felt uneasy in your slumber. After some time the sun was shining again. Somehow, you managed to get out of the forest. A huge path now lies before you. You see the floating cubes in the sky. They are huge and afar. You wonder about the Dark World. Are you here to save the world? That would be inspiring at least. Does it need saving? From what? You abandon your thoughts when you come near a house. There is a familiar figure there…";
                break;
            case "World03(Descent)":
                loreText.text = "\tFor a moment there, your mind had failed you. The image of an Old Knight, waiting in a bright room, emerged in your thoughts. You can’t remember how you ended up here, on the peak of a mountain surrounded by trees and thick mist. You have a feeling that someone or something carried you here. But who, or what? And more important – why? You feel rested as if you slept the night. You saw no dreams again. How long were you asleep?";
                break;
            case "World03(DescentBoss)":
                loreText.text = "\tThis was a long climb down the mountain. You finally reach the bottom and suddenly the sky above you is bright. You find yourself on a field with the road. It seems to lead to some gates. Maybe this is the gates to the Town. As you can recall, this is the only way to reach the Castle. However, the way is not clear. There is something huge beside the gates. You feel a dark presence.";
                break;
            case "World04(Town)":
                loreText.text = "\tThe Guardian is dead. The gates are opening. You venture through to an empty city. It is old and seems forgotten. You travel deep and find yourself standing in front of a house with the light emitting from the windows. There is someone there. However, before you enter - a thought comes to your mind. “Why am I here? What am I doing? What if the Guardian was right and I am the intruder? Though do I have a choice but to venture forth?\"";
                break;
            case "Home":
                loreText.text = "\tThe fight with the Horror was exhausting and you fell on the ground. Tall grass surrounded you and The Darkness spoke. What would become of you now? Are you exposed? What does it need from you? These questions are overwhelming. You could not stand it anymore and your mind failed you once again. You heard a whisper but you could not make out words. The feeling of someone carrying you and mending your wounds swiftly flew in your thoughts. You woke up to the sounds of wind and water. It seems your mind is clear. But where are you?";
                break;
            case "World05(Storm)":
                loreText.text = "\tWas it a dream? It surely looked like one. But you never knew. The sound of thunder and cold raindrops woke you up. You felt freezing and empty, uneasy and troubled though refreshed a little. The storm is rising. You find yourself on a moist road at night, surrounded by dead trees. How did you end up here? The voice of The Darkness still echoes in your mind. The rain and wind are strong and it is getting cold. Time to survive. There is a road sign with the note beside it… ";
                break;
            case "World06(Indoor)":
                loreText.text = "\tThe graves are over, undead are left behind rotting in their loneliness. You are above the sky and there is no storm. It was nice meeting an Old Knight - after all this time you feel like he is your only friend and can be trusted. And even more, you feel you are his friend. The things he speaks of are indeed strange, but you feel comfort after a small chat with him. He was waiting on the top of the stairs and invited you into a strange looking structure, which appears to be floating in the sky. You passed a few dark corridors until you saw a light. The Knight did not follow. There is someone familiar there…";
                break;
            case "IntroSecond":
                loreText.text = "\tWalking down those dark corridors was horrible and felt like eternity. A presence in your mind was haunting you all the time - heavy and dark. The darkness in you grew. You could feel it though you try to deny it. After talking to the Creator you fell asleep. Surely, you did not want to, but you could not help it. Or maybe you just lost track of time? You find yourself in a dark room, the same as when you first arrived in this world… there is someone there…";
                break;
            case "World07(Castle1)":
                loreText.text = "\tOld King? Where can you find him, why? One more riddle is upon you. You are so tired of endless question and no answers. Now you need to look for him. You feel an urgent need to speak to your friend, an Old Knight. He must know something. This time you will demand the answers! But where is he? You are in a bright room. The sound of wind, howling somewhere, and the feeling of despair comes over you… there is something wrong…";
                break;

            case "EndingOldKnight":
                loreText.text = "\tYou surrender… you can’t win, you can’t fight. An Old Knight approaches you. You can see more Dark Knights appearing around you. They grab and drag you across the dim hall. Old Knight walks by. He never looked or spoke at you. You had no words to say either. You are now in the long and dark corridor. The sound of wind is growing. Finally, you reach the end and see an endless pit beneath you. Sky above and giant floating cubes far away. For the last time you wonder – what are they? An Old Knight leans towards grabbing you with his huge and strong arms. You can’t hear his breath but you had a glance into his shining red empty eyes… he looks at you and fear grows inside you. You can feel the darkness. He pushes you down the pit, into the emptiness, and jumps right after. Turning back, you can see that you were pushed from a giant floating cube, similar to those far away. You can’t tell for how long you both were falling, but it was long indeed. Finally, you lose consciousness… ";
                break;

            case "EndingOldKnight2":
                loreText.text = "\tThe silence in darkness...";
                break;

            case "World08(Night)":
                loreText.text = "\tEmptiness. Loneliness. Despair. How could this happen? This can’t be… it has to be a trick, a dream… but it is not! Dark thoughts haunted you while the elevator was going down. And it was going down for a long time. You couldn’t tell how much had passed. A day or two, or maybe five? You could not care less, for your soul was empty now. Finally, the elevator had reached the ground. The night was at height, the loss was over you. Now you saw the Castle far away. The one an Old Knight was talking about. “The Salvation is in the light. You should follow the sun which shines above The Great Castle” You have no desire to continue your journey… but what else is there to do…";
                break;

            case "World09(Castle2)":
                loreText.text = "\tYou crossed the bridge and went through a long, dark and endless corridor. You followed someone here, but you see no sign of this person now. As if he disappeared. Was this just your imagination? Finally, you arrived at the enormous bright room with the giant columns and the sky above your head. You feel the dark presence more than ever. The Darkness is upon you. There is no escape…";
                break;

            case "World10(ThroneRoom1)":
                loreText.text = "\tYou feel nothing. Nothing at all. Did you win? All your journey flew before you. You walked through the castle and did not even notice it. Dark Knights were staring at you with their dead and empty eyes but you did not care and they have not touched you. You walked as if something was driving you. Finally, you realize what it was… there is a throne in the dark room. You saw it before… you heard of it before. The throne of an Old King. It is far away. What will you do?...";
                break;

                //scene if player sit on throne. friends talk
            case "World12(Friends)":
                loreText.text = "\t...";
                break;

                //the ending if player sit on throne (scene after friends talk!!!!!!)
            case "World11(ThroneRoom2)":
                loreText.text = "\tThe dark presence all around you is too heavy to bare. You decide to sit on the throne. And with this though comes a sudden silence. You sit down on the edge of the Throne… and you feel nothing. There is something moving in the far corner of the hall. You can see lights. But you can’t move. You can hear deep breath and see two red eyes in the darkness, among other lights. The Horror approaches you. The one you thought you killed. It does not mind you and just sits by near the Throne. Now you can see the rest of the creatures in the dark. There are Dark Knights, giant skeletons with torches and other monsters. They all stand there as if guarding the hall. One of the Dark Knights approaches you and stands near. Are they guarding you?...";
                break;

          

           
            default:
                break;
        }
        //loreText.GetComponent<TypewriterByVict>().StartTyping(loreText.text);
	}

    void Update() {
        if (pressMouseButt)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartTheScene();
            }

        }
    }
    /// <summary>
    /// Loads your async scene. you need to pass here the name of your next scene!
    /// </summary>
    /// <returns>The your async scene.</returns>
    /// <param name="nameOfScene">Name of scene.</param>
    IEnumerator LoadYourAsyncScene(string nameOfScene)
    {
        //yield return new WaitForSeconds(5);
  
        asyncLoad = SceneManager.LoadSceneAsync(nameOfScene);

        asyncLoad.allowSceneActivation = false;

       
        while (asyncLoad.progress < 0.9f)
        {
           // Debug.Log("progress < 90");
            loadingText.text = "Loading level: " + Mathf.Round (asyncLoad.progress * 100).ToString() + "%";
            slider.value = asyncLoad.progress;
            yield return null;
        }

        pressMouseButt = true;

        while (!allowLoad)
        {
            //Debug.Log("no allow load");
            loadingText.text = "Almost done. Press USE (RMB or A on controller) and wait some time. Don't close the game if it appears to be not responding. It is working." ;
            slider.value = 1;
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        yield return null;

        asyncLoad.allowSceneActivation = true;
        //Debug.Log("done");

    }

    public void StartTheScene() 
    {       
       // Debug.Log("pressed butt");
        allowLoad = true;
    }


}
