------------------------------------------
 WARNING! Breaking changes on CzomPack v3 
------------------------------------------
# Namespace related changes:
- Package moved from `hu.czompisoftware.library.` to `CzomPack.`.
- Changed from lowercase namespaces to PascalCase ones.

# Separated packages:
## We moved some parts of this package to a spearate package to minify this package.
List of separated packages:

|      Current Namespace      |           Package           |
|-----------------------------|-----------------------------|
| `CzomPack.Crypto.*`         | `CzomPack.Cryptography`     |
| `CzomPack.License.*`        | **[DELETED]**               |
| `CzomPack.Network.*`        | `CzomPack.Network`          |
| `CzomPack.NetworkProxy.*`   | `CzomPack.NetworkProxy`     |
| `CzomPack.Translation.*`    | `CzomPack.Translation`      |

# Deprecated features:
- `CzomPack.License` is deprecated and replaced with a separate `ProductActivation` package (internal use only!)
