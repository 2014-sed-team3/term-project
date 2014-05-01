public interface FlyBehavior {
   public void fly();
}
public class FlyWithWings implements FlyBehavior {
       public void fly() {
              System.out.println(“I’m flying!!”);
       }
}
public class FlyNoWay implements FlyBehavior {
       public void fly() {
              System.out.println(“I can’t fly”);
       }
}