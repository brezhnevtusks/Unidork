namespace UnderdorkStudios.UnderTags
{
    public enum UnderTagDatabaseResult
    {
        Success,
        TagStartsWithDot,
        TagEndsWithDot,
        TwoOrMoreDotsInARow,
        InvalidCharacters,
        TagAlreadyExists
    }
}