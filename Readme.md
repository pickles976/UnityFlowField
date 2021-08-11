This is a very barebones FlowField generation script.
FlowFieldController is the Monobehavior script that should be the main
access point for generating FlowFields. A static FlowFieldProvider class
calls FlowFieldFactory and gets a FlowField. This FlowField is in the static
class and can be accessed from any script. This is ideal for my use case since
I just have a single player and a bunch of Zombies that will only ever need to 
move towards the player. This method will probably not be ideal for a lot of 
people. I may fork it in the future if my use cases change-- feel free to change
it yourself though :)

How to use:
1. Clone the Repo to somewhere in your Assets folder
2. Attach the FlowFieldController script to an empty GameObject
3. Set all of the public fields in the script accordingly
4. To get the vector from a Flowfield simply do:
"Vector3 vec = FlowFieldProvider.GetVector(transform.position);"

Since FlowFieldProvider is static, it can be accessed from anywhere. 
GetVector returns the flowfield vector for a given position.
