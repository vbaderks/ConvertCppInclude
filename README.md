# Convert C++ #include "" to <>

## Introduction

ConvertCppInclude is a small C# tool to convert C++ #include "header.h" statements recursively to #include \<header.h>.
It does this by checking if the "header.h" file is in the same directory as the main source code file or not.

The C++ standard is a little fuzzy about the difference between #include "" and #include \<>. But in almost all implementations the
\#include "" statement will look first in the same directory as the source code file that contains the #include statement.
Include files that are in a different folder and can only be found with the include path environment, should preferable be
opened with #include \<>

Using #include "..." for external include headers, are theoretical slower as the preprocessor needs to check every time if the file exists or
not while the header file will never exist in that location. No measurement data has been collected however if this delay is significant or not.
