# unite-cache

Shared architecture, development workflows, and agent instructions:
[UNITE project documentation](https://github.com/dkfz-unite/.github/blob/main/README.md).

`Unite.Cache` is a shared MongoDB supporting-data library, not a server process.
It provides `CacheRepository<T>`, a BSON entity wrapper, and connection options.

Consumers derive repositories and select their database/collection names.
Uses include staged feed submissions, saved dataset presets, and analysis
records. Their review, retention, ownership, and deletion behavior belongs to
the consuming service; the name "cache" does not make all stored data disposable.

The library depends on `MongoDB.Driver` and is used by feeds, Composer,
Analysis, and Identity. It does not supply domain PostgreSQL access or
Elasticsearch search/indexing; those are separate packages.
