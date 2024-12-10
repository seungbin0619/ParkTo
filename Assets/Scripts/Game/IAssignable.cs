public interface IAssignable<T> {
    bool IsAssignable(T t);
    void Assign(T t);
}