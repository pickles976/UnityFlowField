This is a very barebones FlowField generation script.
FlowFieldController is the Monobehavior script that should be the main
access point for generating FlowFields. A static FlowFieldProvider class
calls FlowFieldFactory and gets a FlowField. This FlowField is in the static
class and can be accessed from any script. This is ideal for my use case since
I just have a single player and a bunch of Zombies that will only ever need to 
move towards the player. This method will probably not be ideal for a lot of 
people. I may fork it in the future if my use cases change-- feel free to change
it yourself though :)