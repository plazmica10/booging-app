import pandas as pd
import csv
df = pd.read_csv("Languages.csv", sep=" ,")
print("# INPUT:")
print(df)

# set
df.index.name = "Id"
df = df.rename(columns={"Language name": "Name", "Native name": "Native"})
df["Name"] = df["Name"].str.replace(' "', '', regex=True)
df["Name"] = df["Name"].str.replace('"', '', regex=True)
df["Name"] = df["Name"].str.replace(';', '', regex=True)
df["Name"] = df["Name"].str.replace("'", '', regex=True)
df["Name"] = df["Name"].str.replace(",", '', regex=True)

df["Native"] = df["Native"].str.replace(' "', '', regex=True)
df["Native"] = df["Native"].str.replace('"', '', regex=True)
df["Native"] = df["Native"].str.replace(';', '', regex=True)
df["Native"] = df["Native"].str.replace("'", '', regex=True)
df["Native"] = df["Native"].str.replace(",", '', regex=True)

print("# OUTPUT:")
print(df)
df.to_csv("static_languages.csv", sep='|', quoting=csv.QUOTE_NONE)
