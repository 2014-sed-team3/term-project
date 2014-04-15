public class Line{
    Component[] cmps = new Component[100];
    public Line(int size){
        for(int i=0;i<size;i++)
            cmps[i] = new Component();
    }
}
