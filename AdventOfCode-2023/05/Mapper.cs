namespace AdventOfCode_2023._05;

public record Map(long Source, long Target, long Range)
{
    public bool IsInRange(long input) => input >= Source && input <= Source + Range;
    public long GetTarget(long input) => Target + (input - Source);

    public override string ToString()
    {
        return $"Source: {Source} Target: {Target} Range: {Range}";
    }
};

public class Mapper
{
    private string Type { get; init; }
    private readonly List<Map> _map = new();

    private Mapper(string type)
    {
        Type = type;
    }

    private void AddMap(string line)
    {
        var split = line.Split(" ");
        var target = long.Parse(split[0]);
        var source = long.Parse(split[1]);
        var range = long.Parse(split[2]);

        _map.Add(new Map(source, target, range));
    }

    public static Mapper FromConfiguration(IEnumerable<string> configuration, string keyword)
    {
        var mapper = new Mapper(keyword);

        var keywordFound = false;
        foreach (var line in configuration)
        {
            if (line.Contains(keyword))
            {
                keywordFound = true;
                continue;
            }

            if (!keywordFound) 
                continue;
            
            if (string.IsNullOrEmpty(line))
                break;
                
            mapper.AddMap(line);
        }

        return mapper;
    }

    public long GetTarget(long source)
    {
        var map = _map.FirstOrDefault(x => x.IsInRange(source));
        return map?.GetTarget(source) ?? source;
    }

    public override string ToString()
    {
        return Type + " - " + string.Join(" | ", _map.Select(x => x.ToString()));
    }
}

public class MapperPipeline
{
    private readonly List<Mapper> _mappers = new();
    private readonly List<string> _config;

    public MapperPipeline(List<string> config)
    {
        _config = config;
    }

    public MapperPipeline AddMapper(string keyword)
    {
        _mappers.Add(Mapper.FromConfiguration(_config, keyword));
        return this;
    }

    public long Map(long source)
    {
        return _mappers.Aggregate(source, (current, mapper) => mapper.GetTarget(current));
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, _mappers.Select(x => x.ToString()));
    }
}