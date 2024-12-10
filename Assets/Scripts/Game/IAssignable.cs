public interface IAssignable<T> {
    bool IsAssignable();
    void Assign(T t);
}