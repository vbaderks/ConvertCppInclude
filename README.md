# Remove MFC Comments

## Introduction

ConvertCppInclude is a small C# tool to convert #include "global-file" recursively to #include \<global-file>

The C++ standard is a little fuzzy about the difference between #include "" and #include \<>. But in essence
\#include "" will look first in the same directory as the file that contains the #include statement.
Include files that are in a different folder and can only be found with the include path environment, should preferable be
opended with #include \<>