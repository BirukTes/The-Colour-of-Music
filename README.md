![TCM-Icon (Custom)](https://user-images.githubusercontent.com/23151263/115999184-bbfc5400-a5e2-11eb-8dd2-029b38d0eba0.png)

# The Colour of Music
The Colour of Music - Unity 2D Visualisation demonstration application 

This project is about mapping or translating music into a visual experience such as a 2D colour picture, coloured video or light visualisation using Emotion as the medium (between music and colour). The system is a Windows desktop application built using Unity 3D Game Engine with C# and Sonic Annotator console application with Vamp Plugins for extracting audio data. (Take a below for more details.)


## Application Features
- [x] Detection the emotion of a piece of an audio 
- [x] Display a corresponding colour for a given emotion
- [x] Playing the current audio
- [x] An option to select between a “Video” and “Picture” mode
  - [x] Moving colour change or picture.
  - [x] A still picture, made up of all the screenshots taken during 'Video' mode.
- [x] An option to change between selected audio file
- [x] A save button to save the selected video (recorder is buggy, is unable to delete recorded video and audio after merging to the final output video) or picture
- [x] A switch to full screen or windowed mode

###### Known Issues
- [x] While switching to windowed mode from screen, a second window may appear in front which looks like not responsive:
  - [ ] Change to the first window from the taskbar or alt+tab switcher and close the second window.
  
  
## System Requirements
1. Windows 10 64bit
2. 6 Gigabytes (GB) or more available RAM
3. Other CPU and GPU requirements:
   - https://docs.unity3d.com/2020.3/Documentation/Manual/system-requirements.html
   - ![image](https://user-images.githubusercontent.com/23151263/115573608-ceac1b80-a2b8-11eb-8006-27131c40e25b.png "System requirements for Unity Desktop 2020 LTS")


## Application Interface
- On startup
![Screenshot](https://user-images.githubusercontent.com/23151263/115565017-eb445580-a2b0-11eb-8f6d-9936757c6421.png "App Controls and Logo")

#### Output of Picture Mode
- Test audio files can be found in the releases folder `(\The Colour of Music_Data\StreamingAssets\TCM\Audio)`.
- Each colour square is a screenshot from the 'video'/movie mode.
![The Colour of Music - Erik Satie - Gymnopédie No  1 (Rousseau) - 16-04-2021 14 24 48](https://user-images.githubusercontent.com/23151263/115564399-57728980-a2b0-11eb-9646-305ed4f34948.png "Erik Satie - Gymnopédie No  1 (Rousseau)")
![The Colour of Music - Franz Liszt - Hungarian Rhapsody No   2 (Rousseau) - 16-04-2021 14 28 01](https://user-images.githubusercontent.com/23151263/115564406-580b2000-a2b0-11eb-9241-1268ff2edf0e.png "Franz Liszt - Hungarian Rhapsody No   2 (Rousseau)")
![The Colour of Music - Frédéric François Chopin - Fantaisie-Impromptu (Op  66) (Rousseau) - 16-04-2021 14 31 37](https://user-images.githubusercontent.com/23151263/115564411-58a3b680-a2b0-11eb-8681-ccd06c18050a.png "Frédéric François Chopin - Fantaisie-Impromptu (Op  66) (Rousseau)")
![The Colour of Music - Frédéric François Chopin - Marche Funèbre (Funeral March) (Rousseau) - 16-04-2021 14 34 53](https://user-images.githubusercontent.com/23151263/115564413-58a3b680-a2b0-11eb-94e9-132798be59f3.png "Frédéric François Chopin - Marche Funèbre (Funeral March) (Rousseau)")
![The Colour of Music - Ludwig van Beethoven - Für Elise Bagatelle No  25 in A Minor, WoO 59 (Lang Lang) - 16-04-2021 14 06 30](https://user-images.githubusercontent.com/23151263/115564415-593c4d00-a2b0-11eb-8a2d-7b8c08c4f5d8.png "Ludwig van Beethoven - Für Elise Bagatelle No  25 in A Minor, WoO 59 (Lang Lang)")
![The Colour of Music - Ludwig van Beethoven - Moonlight Sonata (1st Movement) (Rousseau) - 16-04-2021 14 11 38](https://user-images.githubusercontent.com/23151263/115564416-593c4d00-a2b0-11eb-83da-59f0559c3acd.png "Ludwig van Beethoven - Moonlight Sonata (1st Movement) (Rousseau)")
![The Colour of Music - Nikolai Rimsky-Korsakov (arr  Sergei Vasilyevich Rachmaninoff) - Flight of the Bumblebee (Rousseau) - 16-04-2021 14 39 28](https://user-images.githubusercontent.com/23151263/115564420-593c4d00-a2b0-11eb-9891-45ea1adb470d.png "Nikolai Rimsky-Korsakov (arr  Sergei Vasilyevich Rachmaninoff) - Flight of the Bumblebee (Rousseau)")
![The Colour of Music - Sergei Vasilyevich Rachmaninoff - Prelude in C Sharp Minor (Op  3 No  2) (Rousseau) - 16-04-2021 14 42 43](https://user-images.githubusercontent.com/23151263/115564423-59d4e380-a2b0-11eb-822b-bf8ff9616685.png "Sergei Vasilyevich Rachmaninoff - Prelude in C Sharp Minor (Op  3 No  2) (Rousseau)")
![The Colour of Music - Wolfgang Amadeus Mozart - Rondo Alla Turca (Marnie Laird) - 16-04-2021 14 45 11](https://user-images.githubusercontent.com/23151263/115564425-59d4e380-a2b0-11eb-9593-08344c51254e.png "Wolfgang Amadeus Mozart - Rondo Alla Turca (Marnie Laird)")
![The Colour of Music - Claude Debussy - Clair de Lune (Rousseau) - 16-04-2021 14 19 54](https://user-images.githubusercontent.com/23151263/115564426-5a6d7a00-a2b0-11eb-94b9-e9cbb9275499.png "Claude Debussy - Clair de Lune (Rousseau)")


## How it works?
1. Music is processed using `Sonic Annotator` to extract the `Tempo` and `Mode`, provided as JSON format/type. 
2. Then as the music plays, the time position is taken, using that the `Tempo` and `Mode` are looked-up and saved into current... variables.
3. Using the current tempo and mode the emotion is determined.    
4. Using the emotion the corresponding colour is shown using a LERP (Linear Interpolation) function to ease it.
5. There is also intensity Synchronisation which corresponds the current tempo, modifies the intensity of the colour shown.
![image](https://user-images.githubusercontent.com/23151263/116756435-fd866800-aa03-11eb-9204-233c3b17d2a8.png)

### Music and Emotion Mapping
- Emotion categories and methods are derived from the paper by (Ramos et al., 2011).
![image](https://user-images.githubusercontent.com/23151263/116757968-19d7d400-aa07-11eb-8f3e-0af51c466d34.png)


### Emotion and Colour Mapping
- The emotion to colour map was created in accordance with (Art Therapy, 2011) and (Ram et al., 2020).
![image](https://user-images.githubusercontent.com/23151263/116758048-4390fb00-aa07-11eb-8400-0e7d76cc196b.png)


## Results
1. After running the test cases, it was evident that the demonstrative system was able to detect emotion and display the corresponding colour successfully.
2. It was shown that some elements of Für Elise (Beethoven) were captured as 2D colour picture.
![image](https://user-images.githubusercontent.com/23151263/116759067-50aee980-aa09-11eb-986c-9a95dc76d4a6.png)
![image](https://user-images.githubusercontent.com/23151263/116759026-3ecd4680-aa09-11eb-8523-8e5e091c3e70.png)
![image](https://user-images.githubusercontent.com/23151263/116759511-6670de80-aa0a-11eb-83a3-15020dda70c7.png)
![image](https://user-images.githubusercontent.com/23151263/116759049-4856ae80-aa09-11eb-8232-d1e276ed0f3e.png)

## Limitations and Future Work
1. Emotion detection and use of colour palette were some of the limitations of this project and other previous projects due to ambiguities, subjectivities, and disagreements that exist (Yang & Chen, 2012).
2. Emotion detection could possibly use newer methods of MER which are mostly based of Machine Learning technology.
3. This would eliminate use of Sonic Annotator and conditionals and broaden the application of the system.
4. Colour changing could match the beat/tempo, currently it is not convincing that it does use the tempo multipler.
5. The prototype application could use less system resources, therefore being able to run and adapt to the length of the audio music played.
6. The video recording and saving feature could use the newer or another enhanced library.


## References
- RAMOS, D., BUENO, J.L.O. & BIGAND, E., 2011. Manipulating Greek musical modes and tempo affects perceived musical emotion in musicians and nonmusicians. Brazilian Journal of Medical and Biological Research. 44(2), pp.165-172. Available from: 10.1590/S0100-879X2010007500148.
- RAM, V., SCHAPOSNIK, L.P., KONSTANTINOU, N., VOLKAN, E., PAPADATOU-PASTOU, M., MANAV, B., JONAUSKAITE, D. & MOHR, C., 2020. Extrapolating continuous color emotions through deep learning. Physical Review Research. 2(3), pp.033350. Available from: https://doi.org/10.1103/PhysRevResearch.2.033350.
- ART THERAPY. ,Color Psychology: The Psychological Effects of Colors, 2011.: Art Therapy. -02-28T06:49:08+00:00 [viewed Apr 4, 2021]. Available from: http://www.arttherapyblog.com/online/color-psychology-psychologica-effects-of-colors/.

- YANG, Y. & CHEN, H.H., 2012. Machine Recognition of Music Emotion. ACM Transactions on Intelligent Systems and Technology (TIST). 3(3), pp.1-30. Available from: 10.1145/2168752.2168754.
- PALMER, S.E., SCHLOSS, K.B., XU, Z. & PRADO-LEÓN, L.R., 2013. Music–color associations are mediated by emotion. Proceedings of the National Academy of Sciences of the United States of America. 110(22), pp.8836-8841. Available from: 10.1073/pnas.1212562110.
- WHITEFORD, K.L., SCHLOSS, K.B., HELWIG, N.E. & PALMER, S.E., 2018. Color, Music, and Emotion: Bach to the Blues. I-Perception (London). 9(6), pp.204166951880853-2041669518808535. Available from: 10.1177/2041669518808535.
