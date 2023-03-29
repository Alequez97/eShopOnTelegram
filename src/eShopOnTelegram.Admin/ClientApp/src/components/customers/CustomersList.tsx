import { List, Datagrid, TextField } from "react-admin";

export default function CustomersList() {
  return (
    <List>
      <Datagrid>
        <TextField
          source="telegramUserUID"
          label="Telegram User UID"
          sortable={false}
        />
        <TextField source="username" sortable={false} emptyText={"-"} />
        <TextField source="firstName" sortable={false} />
        <TextField source="lastName" sortable={false} emptyText={"-"} />
      </Datagrid>
    </List>
  );
}
