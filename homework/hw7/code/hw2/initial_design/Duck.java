public abstract class Duck {
   FlyBehavior flyBehavior;
   QuackBehavior quackBehavior;
   public Duck() {
   }
   public abstract void display();
   public abstract void modified_FlyBehavior(FlyBehavior fly);
   public abstract void modified_QuackBehavior(QuackBehavior qq)
   public void doFly() {
          flyBehavior.fly();
   }
   public void doQuack() {
          quackBehavior.quack();
   }
   public void swim() {
          System.out.println(“All ducks float, even decoys!”);
   }
}

//綠頭鴨
public class MallardDuck extends Duck {
       public RedheadDuck() {
       }
       public void modified_FlyBehavior(FlyBehavior fly) {
       	       flyBehavior = fly;
       }
       public void modified_QuackBehavior(QuackBehavior qq) {
       	       quackBehavior = qq;
       }
       public void display() {
              System.out.println(“I’m a model duck”);
       }
}
//紅頭鴨
public class RedheadDuck extends Duck {
       public RedheadDuck() {
       }
       public void modified_FlyBehavior(FlyBehavior fly) {
       	       flyBehavior = fly;
       }
       public void modified_QuackBehavior(QuackBehavior qq) {
       	       quackBehavior = qq;
       }
       public void display() {
              System.out.println(“I’m a model duck”);
       }
}
//橡皮鴨
public class RubberDuck extends Duck {
       public RedheadDuck() {
       }
       public void modified_FlyBehavior(FlyBehavior fly) {
       	       flyBehavior = fly;
       }
       public void modified_QuackBehavior(QuackBehavior qq) {
       	       quackBehavior = qq;
       }
       public void display() {
              System.out.println(“I’m a model duck”);
       }
}
//誘餌鴨
public class DecoyDuck extends Duck {
       public RedheadDuck() {
       }
       public void modified_FlyBehavior(FlyBehavior fly) {
       	       flyBehavior = fly;
       }
       public void modified_QuackBehavior(QuackBehavior qq) {
       	       quackBehavior = qq;
       }
       public void display() {
              System.out.println(“I’m a model duck”);
       }
}