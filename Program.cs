
// Создаем бинарное дерево
IAbstractBinaryTree<int> tree = BuildSampleTree();

tree.Print();

// Выводим результаты обходов в консоль
Console.WriteLine("\nPreOrder:");
PrintTreeList(tree.PreOrder());

Console.WriteLine("\nInOrder:");
PrintTreeList(tree.InOrder());

Console.WriteLine("\nPostOrder:");
PrintTreeList(tree.PostOrder());

Console.WriteLine("\nBreadthFirstSearch:");
PrintTreeList(tree.BreadthFirstSearch());

Console.WriteLine("\nDepthFirstSearch:");
PrintTreeList(tree.DepthFirstSearch());

// Применяем Consumer к каждому элементу и выводим результат в консоль
Console.WriteLine("\nForEachInOrder:");
tree.ForEachInOrder(key => Console.Write(key + " "));


// Вспомогательный метод для построения примера бинарного дерева
static IAbstractBinaryTree<int> BuildSampleTree()
{
    // Строим дерево:        1
    //                     /   \
    //                    2     3
    //                   / \   / \
    //                  4   5 6   7
    var node15 = new BinaryTree<int>(15);
    var node14 = new BinaryTree<int>(14);
    var node13 = new BinaryTree<int>(13);
    var node12 = new BinaryTree<int>(12);
    var node11 = new BinaryTree<int>(11);
    var node10 = new BinaryTree<int>(10);
    var node9 = new BinaryTree<int>(9);
    var node8 = new BinaryTree<int>(8);
    var node7 = new BinaryTree<int>(7) { Left = node14, Right = node15 };
    var node6 = new BinaryTree<int>(6) { Left = node12, Right = node13 };
    var node5 = new BinaryTree<int>(5) { Left = node10, Right = node11 };
    var node4 = new BinaryTree<int>(4) { Left = node8, Right = node9 };
    var node3 = new BinaryTree<int>(3) { Left = node6, Right = node7 };
    var node2 = new BinaryTree<int>(2) { Left = node4, Right = node5 };
    var node1 = new BinaryTree<int>(1) { Left = node2, Right = node3 };

    return node1;
}

// Вспомогательный метод для вывода списка узлов в консоль
static void PrintTreeList(List<IAbstractBinaryTree<int>> list)
{
    foreach (var node in list)
    {
        Console.Write(node.GetKey() + " ");
    }
    Console.WriteLine();
}




public class BinaryTree<E> : IAbstractBinaryTree<E>
{
    public E Key { get; set; }
    public IAbstractBinaryTree<E> Left { get; set; }
    public IAbstractBinaryTree<E> Right { get; set; }

    public BinaryTree(E key)
    {
        Key = key;
        Left = null;
        Right = null;
    }

    public E GetKey()
    {
        return Key;
    }

    public IAbstractBinaryTree<E> GetLeft()
    {
        return Left;
    }

    public IAbstractBinaryTree<E> GetRight()
    {
        return Right;
    }

    public void SetKey(E key)
    {
        Key = key;
    }

    public string AsIndentedPreOrder(int indent)
    {
        string indentation = new string(' ', indent * 2);
        return $"{indentation}{Key}\n" +
               $"{(Left != null ? Left.AsIndentedPreOrder(indent + 1) : "")}" +
               $"{(Right != null ? Right.AsIndentedPreOrder(indent + 1) : "")}";
    }

    public List<IAbstractBinaryTree<E>> PreOrder()
    {
        List<IAbstractBinaryTree<E>> result = new List<IAbstractBinaryTree<E>>();
        result.Add(this);
        if (Left != null)
            result.AddRange(Left.PreOrder());
        if (Right != null)
            result.AddRange(Right.PreOrder());
        return result;
    }

    public List<IAbstractBinaryTree<E>> InOrder()
    {
        List<IAbstractBinaryTree<E>> result = new List<IAbstractBinaryTree<E>>();
        if (Left != null)
            result.AddRange(Left.InOrder());
        result.Add(this);
        if (Right != null)
            result.AddRange(Right.InOrder());
        return result;
    }

    public List<IAbstractBinaryTree<E>> PostOrder()
    {
        List<IAbstractBinaryTree<E>> result = new List<IAbstractBinaryTree<E>>();
        if (Left != null)
            result.AddRange(Left.PostOrder());
        if (Right != null)
            result.AddRange(Right.PostOrder());
        result.Add(this);
        return result;
    }

    public void ForEachInOrder(Action<E> consumer)
    {
        if (Left != null)
            Left.ForEachInOrder(consumer);
        consumer(Key);
        if (Right != null)
            Right.ForEachInOrder(consumer);
    }

    public List<IAbstractBinaryTree<E>> DepthFirstSearch()
    {
        var result = new List<IAbstractBinaryTree<E>>();
        DepthFirstSearchHelper(this, result);
        return result;
    }

    private void DepthFirstSearchHelper(IAbstractBinaryTree<E> node, List<IAbstractBinaryTree<E>> result)
    {
        result.Add(node);
        if (node.GetLeft() != null)
            DepthFirstSearchHelper(node.GetLeft(), result);
        if (node.GetRight() != null)
            DepthFirstSearchHelper(node.GetRight(), result);
    }

    public List<IAbstractBinaryTree<E>> BreadthFirstSearch()
    {
        var result = new List<IAbstractBinaryTree<E>>();
        var queue = new Queue<IAbstractBinaryTree<E>>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            if (current.GetLeft() != null)
                queue.Enqueue(current.GetLeft());
            if (current.GetRight() != null)
                queue.Enqueue(current.GetRight());
        }

        return result;
    }


}

public interface IAbstractBinaryTree<E>
{
    E GetKey();
    IAbstractBinaryTree<E> GetLeft();
    IAbstractBinaryTree<E> GetRight();
    void SetKey(E key);
    string AsIndentedPreOrder(int indent);
    List<IAbstractBinaryTree<E>> PreOrder();
    List<IAbstractBinaryTree<E>> InOrder();
    List<IAbstractBinaryTree<E>> PostOrder();
    void ForEachInOrder(Action<E> consumer);
    List<IAbstractBinaryTree<E>> DepthFirstSearch();
    List<IAbstractBinaryTree<E>> BreadthFirstSearch();
}




public static class BinaryTreePrinter
{
    class NodeInfo
    {
        public IAbstractBinaryTree<int> Node;
        public string Text;
        public int StartPos;
        public int Size { get { return Text.Length; } }
        public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
        public NodeInfo Parent, Left, Right;
    }

    public static void Print(this IAbstractBinaryTree<int> root, int topMargin = 2, int leftMargin = 2)
    {
        if (root == null) return;
        int rootTop = Console.CursorTop + topMargin;
        var last = new List<NodeInfo>();
        var next = root;
        for (int level = 0; next != null; level++)
        {
            var item = new NodeInfo { Node = next, Text = next.GetKey().ToString(" 0 ") };
            if (level < last.Count)
            {
                item.StartPos = last[level].EndPos + 1;
                last[level] = item;
            }
            else
            {
                item.StartPos = leftMargin;
                last.Add(item);
            }
            if (level > 0)
            {
                item.Parent = last[level - 1];
                if (next == item.Parent.Node.GetLeft())
                {
                    item.Parent.Left = item;
                    item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                }
                else
                {
                    item.Parent.Right = item;
                    item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                }
            }
            next = next.GetLeft() ?? next.GetRight();
            for (; next == null; item = item.Parent)
            {
                Print(item, rootTop + 2 * level);
                if (--level < 0) break;
                if (item == item.Parent.Left)
                {
                    item.Parent.StartPos = item.EndPos;
                    next = item.Parent.Node.GetRight();
                }
                else
                {
                    if (item.Parent.Left == null)
                        item.Parent.EndPos = item.StartPos;
                    else
                        item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                }
            }
        }
        Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
    }

    private static void Print(NodeInfo item, int top)
    {
        SwapColors();
        Print(item.Text, top, item.StartPos);
        SwapColors();
        if (item.Left != null)
            PrintLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
        if (item.Right != null)
            PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
    }

    private static void PrintLink(int top, string start, string end, int startPos, int endPos)
    {
        Print(start, top, startPos);
        Print("─", top, startPos + 1, endPos);
        Print(end, top, endPos);
    }

    private static void Print(string s, int top, int left, int right = -1)
    {
        Console.SetCursorPosition(left, top);
        if (right < 0) right = left + s.Length;
        while (Console.CursorLeft < right) Console.Write(s);
    }

    private static void SwapColors()
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = Console.BackgroundColor;
        Console.BackgroundColor = color;
    }
}
