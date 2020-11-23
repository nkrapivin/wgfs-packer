# wgfs-packer
A packer for the WGFS (Weird Game File System) format.

## Format structure

```
[4 byte int] Header 'WGFS'
[4 byte int] Format Version (currently 1)

File table:
[4 byte int] Header 'FILE'
[4 byte int] Amount of files
[for i = 0; i < Amount; ++i]
[
  [Null-terminated ASCII string] File name
  [4 byte uint] File size in bytes
  [size] File Data
]

String table:
[4 byte int] Header 'STRG'
[4 byte int] Amount of strings
[for i = 0; i < Amount; ++i]
[
  [Null-terminated ASCII string] Key
  [Null-terminated ASCII string] Value
]

[4 byte int] Header 'WEND'
End of file.
```
