using System;
using System.Collections.Generic;


public class AsmRPN {
  private Dictionary<String, Action> actions;
  private Stack<long> stack;
  private int pc;

  public AsmRPN() {
    actions = new Dictionary<String, Action>() {
      {"add", this.Add},
      {"sub", this.Sub},
      {"mul", this.Mul},
      {"div", this.Div},
      {"dup", this.Dup},
      {"pop", this.Pop},
      {"put", this.Put},
      {"putln", this.PutLn},
      {"get", this.Get},
      {"<", this.LT},
      {"<=", this.LE},
      {"==", this.EQU},
      {">=", this.GE},
      {">", this.GT},
      {"and", this.And},
      {"or", this.Or},
      {"not", this.Not},
      {"jump", this.Jump},
      {"jif", this.Jif},
    };

    stack = new Stack<long>();
  }

  private void Add() {
    stack.Push(stack.Pop() + stack.Pop());
  }

  private void Sub() {
    stack.Push(-stack.Pop() + stack.Pop());
  }

  private void Mul() {
    stack.Push(stack.Pop() * stack.Pop());
  }

  private void Div() {
    stack.Push((long) (1.0/stack.Pop() * stack.Pop()));
  }

  private void Dup() {
    stack.Push(stack.Peek());
  }

  private void Pop() {
    stack.Pop();
  }

  private void LT() {
    stack.Push(Convert.ToInt64(stack.Pop() > stack.Pop()));
  }

  private void LE() {
    stack.Push(Convert.ToInt64(stack.Pop() >= stack.Pop()));
  }

  private void EQU() {
    stack.Push(Convert.ToInt64(stack.Pop() == stack.Pop()));
  }

  private void GE() {
    stack.Push(Convert.ToInt64(stack.Pop() <= stack.Pop()));
  }

  private void GT() {
    stack.Push(Convert.ToInt64(stack.Pop() < stack.Pop()));
  }

  private void And() {
    long a = stack.Pop();
    long b = stack.Pop();

    stack.Push(Convert.ToInt64(a != 0 && b != 0));
  }

  private void Or() {
    long a = stack.Pop();
    long b = stack.Pop();
    stack.Push(Convert.ToInt64(a != 0 || b != 0));
  }

  private void Not() {
    stack.Push(Convert.ToInt64(stack.Pop() == 0));
  }

  private void Jump() {
    pc = Convert.ToInt32(stack.Pop());
  }

  private void Jif() {
    int to = Convert.ToInt32(stack.Pop());

    if (stack.Peek() != 0) {
      pc = to;
    }
  }

  private void Put() {
    Console.Write(stack.Peek());
  }

  private void PutLn() {
    Console.WriteLine(stack.Peek());
  }

  private void Get() {
    stack.Push(long.Parse(Console.ReadLine()));
  }

  public long evaluate(String code) {
    String[] words = code.Split(' ');
    pc = 0;

    while (pc < words.Length) {
      String word = words[pc ++];

      if (actions.ContainsKey(word)) {
        actions[word]();
      } else {
        stack.Push(long.Parse(word));
      }
    }

    return stack.Peek();
  }
}


public class App {
  public static void Main() {

    var asm = new AsmRPN();
    asm.evaluate("get get and putln");
  }
}
