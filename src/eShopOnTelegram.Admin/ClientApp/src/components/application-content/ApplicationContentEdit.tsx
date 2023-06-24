import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNotify, TextInput, SimpleForm, useRefresh } from "react-admin";

type ApplicationContent = Record<string, string>;

const ApplicationContentEdit: React.FC = () => {
  const [applicationContent, setApplicationContent] =
    useState<ApplicationContent | null>(null);

  const notify = useNotify();
  const refresh = useRefresh();

  useEffect(() => {
    const fetchApplicationContent = async () => {
      try {
        const { data } = await axios.get("/applicationContent");
        setApplicationContent(data);
      } catch (error) {
        notify("Error fetching localization data", { type: "error" });
      }
    };

    fetchApplicationContent();
  }, [notify]);

  const handleSave = async () => {
    console.log("save");
    try {
      if (applicationContent) {
        axios.patch("/applicationContent");
        notify("Application content data saved", { type: "success" });
        refresh();
      }
    } catch (error) {
      notify("Error saving localization data", { type: "error" });
    }
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setApplicationContent((prevData) => ({
      ...(prevData as ApplicationContent),
      [name]: value,
    }));
  };

  if (!applicationContent) {
    return <div>Loading...</div>;
  }

  return (
    <SimpleForm onSubmit={handleSave}>
      {Object.entries(applicationContent).map(([key, value], index, array) => {
        if (
          index === 0 ||
          key.split(".")[0] !== array[index - 1][0].split(".")[0]
        ) {
          const groupName = key.split(".")[0];

          return (
            <React.Fragment key={groupName}>
              <h2>{groupName}</h2> {/* Render the group name */}
              <TextInput
                key={key}
                source={key}
                label={key
                  .split(".")[1]
                  .split(/(?=[A-Z])/)
                  .map((word, index) =>
                    index === 0 ? word : word.toLowerCase()
                  )
                  .join(" ")}
                defaultValue={value}
                onChange={handleInputChange}
                fullWidth
              />
            </React.Fragment>
          );
        }

        return (
          <TextInput
            key={key}
            source={key}
            label={key
              .split(".")[1]
              .split(/(?=[A-Z])/)
              .map((word, index) => (index === 0 ? word : word.toLowerCase()))
              .join(" ")}
            defaultValue={value}
            onChange={handleInputChange}
            fullWidth
          />
        );
      })}
    </SimpleForm>
  );
};

export default ApplicationContentEdit;
