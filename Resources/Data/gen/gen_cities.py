import pandas as pd
df = pd.read_csv("world-cities.csv", sep=",")
print("# INPUT:")
print(df)

# set
df.index.name = "Id"
df = df[["geonameid", "country", "subcountry", "name"]]
df = df.rename(columns={
	"geonameid": "GeoId",
	"country": "Country",
	"subcountry": "SubCountry",
	"name": "City"
})

df["Country"] = df["Country"].str.replace('"', '', regex=True)
df["SubCountry"] = df["SubCountry"].str.replace('"', '', regex=True)
df["City"] = df["City"].str.replace('"', '', regex=True)


print("# OUTPUT:")
print(df)
df.to_csv("static_worldcities.csv", sep='|')